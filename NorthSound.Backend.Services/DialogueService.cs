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
    private readonly IConnectionManager _connectionManager;
    private readonly IAccountService _accountService;
    private readonly ApplicationContext _context;

    public DialogueService(
        IConnectionManager connectionManager,
        IAccountService accountService,
        ApplicationContext context)
    {
        _connectionManager = connectionManager;
        _accountService = accountService;
        _context = context;
    }

    public async Task<GenericResponse<Message>> PrepareMessageForSendingAsync(MessageViewModel model, string senderConnectionId)
    {
        ChatUser? receiver = _connectionManager.GetChatUserByUsername(model.ReceiverUsername);
        ChatUser? sender = _connectionManager.GetChatUserByConnectionId(senderConnectionId);

        if (receiver is null || sender is null)
            return Failed<Message>("Пользователь не найден");

        var message = new Message
        {
            Receiver = receiver,
            Sender = sender,
            MessageData = model.Message,
        };

        var dialogueDTO = await GetDialogueBetweenAsync(sender.CurrentUser, receiver.CurrentUser);
        await AddMessageAsync(message, dialogueDTO);

        return Success(message);
    }

    public async Task<GenericResponse<ChatUser>> AddChatUserAsync(ClaimsPrincipal userClaims, string connectionId)
    {
        var usernameClaim = userClaims.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
        var existingUser = await _accountService.GetUserByNameAsync(usernameClaim!.Value);

        if (existingUser is null)
            return Failed<ChatUser>("Пользователь не найден!");

        var addedChatUser = _connectionManager.AddUser(existingUser, connectionId);

        if (addedChatUser is null)
            return Failed<ChatUser>("Пользователь уже существует!");

        return Success(addedChatUser);
    }

    public void RemoveChatUser(string connectionId)
    {
        _connectionManager.RemoveUser(connectionId);
    }

    private async Task<DialogueDTO> GetDialogueBetweenAsync(UserDTO firstUser, UserDTO secondUser)
    {
        DialogueDTO? existingDialogue = await _context.Dialogues.FirstOrDefaultAsync(dialogue
                => (dialogue.FirstUser.Id == firstUser.Id   && dialogue.SecondUser.Id == secondUser.Id)
                || (dialogue.FirstUser.Id == secondUser.Id  && dialogue.SecondUser.Id == firstUser.Id));

        if (existingDialogue is not null)
            return existingDialogue;

        var newDialogue = new DialogueDTO
        {
            FirstUser = firstUser,
            SecondUser = secondUser,
            CreatedAt = DateTime.UtcNow,
        };

        var createdEntry = await _context.Dialogues.AddAsync(newDialogue);
        return createdEntry.Entity;
    }

    private async Task AddMessageAsync(Message message, DialogueDTO dialogue)
    {
        var messageDTO = new MessageDTO
        {
            Sender = message.Sender.CurrentUser,
            Receiver = message.Receiver.CurrentUser,
            Message = message.MessageData,
            Dialogue = dialogue,
            CreatedAt = DateTime.UtcNow,
        };

        await _context.Messages.AddAsync(messageDTO);
    }

    private static GenericResponse<T> Success<T>(T data)
        => GenericResponse<T>.Success(data);

    private static GenericResponse<T> Failed<T>(string details) 
        => GenericResponse<T>.Failed(details);
}
