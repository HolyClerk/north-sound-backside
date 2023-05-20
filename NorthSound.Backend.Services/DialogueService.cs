using Microsoft.EntityFrameworkCore;
using NorthSound.Backend.DAL;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NorthSound.Backend.Services;

public class DialogueService : IDialogueService
{
    private readonly IChatSessions _sessions;
    private readonly IAccountService _accountService;
    private readonly ApplicationContext _context;

    public DialogueService(
        IAccountService accountService,
        ApplicationContext context)
    {
        _accountService = accountService;
        _context = context;

        _sessions = new ChatSessionsHandler();
    }

    public async Task<MessageResponse> PrepareMessageAsync(MessageRequest request)
    {
        // Первоначально пробуется создать сообщение в базе данных,
        // а уже после - проверка возможности отправки к конечному пользователю.
        var createdMessage = await CreateMessageInDatabaseAsync(request);

        if (createdMessage is null)
            return MessageResponse.Failed("Не удалось создать сообщение в базе данных");

        return BuildMessageResponse(createdMessage);
    }

    public async Task<GenericResponse<ChatUser>> AddChatUserAsync(ClaimsPrincipal userClaims, string connectionId)
    {
        var usernameClaim = userClaims.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name);
        var existingUser = await _accountService.GetUserByNameAsync(usernameClaim!.Value);

        if (existingUser is null)
            return Failed<ChatUser>("Пользователь не найден!");

        var addedChatUser = _sessions.AddUser(existingUser, connectionId);

        if (addedChatUser is null)
            return Failed<ChatUser>("Пользователь уже существует!");

        return Success(addedChatUser);
    }

    public bool RemoveChatUser(string connectionId)
        => _sessions.RemoveUser(connectionId);
    
    private async Task<MessageDTO?> CreateMessageInDatabaseAsync(MessageRequest request)
    {
        User? sender = _sessions.GetChatUserByConnectionId(request.SenderConnectionId)?.CurrentUser;
        User? receiver = await _accountService.GetUserByNameAsync(request.ReceiverUsername);

        // Если получать/отправлять некому
        if (sender is null || receiver is null)
            return null;

        var message = new MessageDTO(receiver, sender, request.Message);
        // Создается или получается существующий диалог между пользователями
        var dialogue = await AddDialogueBetweenAsync(sender, receiver);
        // Сообщение сохраняется в базу данных с данными о диалоге
        await AddMessageAsync(message, dialogue);
        await _context.SaveChangesAsync();

        return message;
    }

    private MessageResponse BuildMessageResponse(MessageDTO message)
    {
        var senderChatUser = _sessions.GetChatUserByUsername(message.Sender.Name);
        var receiverChatUser = _sessions.GetChatUserByUsername(message.Receiver.Name);

        if (receiverChatUser is null || senderChatUser is null)
            return MessageResponse.Failed("Пользователь оффлайн");

        return MessageResponse.Success(senderChatUser, receiverChatUser, message);
    }

    private async Task<Dialogue> AddDialogueBetweenAsync(User firstUser, User secondUser)
    {
        Dialogue? existingDialogue = await _context.Dialogues
            .AsNoTracking()
            .FirstOrDefaultAsync(dialogue => 
                (dialogue.FirstUser.Id == firstUser.Id   && dialogue.SecondUser.Id == secondUser.Id) || 
                (dialogue.FirstUser.Id == secondUser.Id  && dialogue.SecondUser.Id == firstUser.Id));

        if (existingDialogue is not null)
            return existingDialogue;

        var newDialogue = new Dialogue
        {
            FirstUserId = firstUser.Id,
            SecondUserId = secondUser.Id,
            CreatedAt = DateTime.UtcNow,
        };

        await _context.Dialogues.AddAsync(newDialogue);
        await _context.SaveChangesAsync();
        return newDialogue;
    }

    private async Task AddMessageAsync(MessageDTO message, Dialogue dialogue)
    {
        var messageDTO = new Message
        {
            SenderId = message.Sender.Id,
            ReceiverId = message.Receiver.Id,
            Text = message.Value,
            DialogueId = dialogue.Id,
            CreatedAt = DateTime.UtcNow,
        };

        await _context.Messages.AddAsync(messageDTO);
    }

    private static GenericResponse<T> Success<T>(T data)
        => GenericResponse<T>.Success(data);

    private static GenericResponse<T> Failed<T>(string details) 
        => GenericResponse<T>.Failed(details);
}
