using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NorthSound.Backend.Services;

public class ChatService : IChatService
{
    private readonly IChatSessions _sessions;
    private readonly IAccountService _accountService;
    private readonly IDialogueService _dialogueService;

    public ChatService(
        IAccountService accountService,
        IDialogueService dialogueService,
        IChatSessions sessions)
    {
        _accountService = accountService;
        _dialogueService = dialogueService;
        _sessions = sessions;
    }

    public async Task<MessageResponse> BuildMessageAsync(MessageRequest request)
    {
        var createdMessage = await CreateMessageAsync(request);

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
    
    private async Task<Message?> CreateMessageAsync(MessageRequest request)
    {
        Dialogue? dialogue;
        User? sender = _sessions.GetChatUserByConnectionId(request.SenderConnectionId)?.CurrentUser;
        User? receiver = await _accountService.GetUserByNameAsync(request.ReceiverUsername);

        if (sender is null || receiver is null)
            return null;

        var message = new MessageDTO(receiver, sender, request.Message);

        dialogue = await _dialogueService.GetDialogueAsync(receiver, sender);
        dialogue ??= await _dialogueService.AddDialogueAsync(sender, receiver);

        return await _dialogueService.AddMessageAsync(message, dialogue); 
    }

    private MessageResponse BuildMessageResponse(Message message)
    {
        var senderChatUser = _sessions.GetChatUserByUsername(message.Sender.Name);
        var receiverChatUser = _sessions.GetChatUserByUsername(message.Receiver.Name);

        if (receiverChatUser is null || senderChatUser is null)
            return MessageResponse.Failed("Пользователь оффлайн");

        var messageDTO = new MessageDTO(message.Receiver, message.Sender, message.Text);
        return MessageResponse.Success(senderChatUser, receiverChatUser, messageDTO);
    }

    private static GenericResponse<T> Success<T>(T data)
        => GenericResponse<T>.Success(data);

    private static GenericResponse<T> Failed<T>(string details) 
        => GenericResponse<T>.Failed(details);
}
