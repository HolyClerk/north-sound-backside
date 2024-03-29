﻿using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NorthSound.Backend.Services;

public class ChatService : IChatService
{
    private readonly ISessions _sessions;
    private readonly IAccountService _accountService;
    private readonly IDialogueService _dialogueService;

    public ChatService(
        IAccountService accountService,
        IDialogueService dialogueService,
        ISessions sessions)
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

    public async Task<GenericResponse<Session>> CreateSessionAsync(ClaimsPrincipal userClaims, string connectionId)
    {
        var usernameClaim = userClaims.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name);
        var existingUser = await _accountService.GetUserByNameAsync(usernameClaim!.Value);

        if (existingUser is null)
            return Failed<Session>("Пользователь не найден!");

        var addedChatUser = _sessions.AddSession(existingUser, connectionId);

        if (addedChatUser is null)
            return Failed<Session>("Пользователь уже существует!");

        return Success(addedChatUser);
    }

    public GenericResponse<Session> GetSession(string connectionId)
    {
        var user = _sessions.GetSession(connectionId);

        if (user is null)
            return GenericResponse<Session>.Failed("Пользователь не найден!");

        return GenericResponse<Session>.Success(user);
    }

    public IEnumerable<Session> GetSessions()
        => _sessions.GetAllSessions();
    
    public bool RemoveSession(string connectionId)
        => _sessions.RemoveSession(connectionId);
    
    private async Task<Message?> CreateMessageAsync(MessageRequest request)
    {
        Dialogue? dialogue;
        User? sender = _sessions.GetSession(request.SenderConnectionId)?.CurrentUser;
        User? receiver = await _accountService.GetUserByNameAsync(request.ReceiverUsername);

        if (sender is null || receiver is null)
            return null;

        var message = new MessageDTO(sender, receiver, request.Message);

        dialogue = await _dialogueService.GetDialogueAsync(sender, receiver);
        dialogue ??= await _dialogueService.AddDialogueAsync(sender, receiver);

        return await _dialogueService.AddMessageAsync(message, dialogue); 
    }

    private MessageResponse BuildMessageResponse(Message message)
    {
        var senderChatUser = _sessions.GetSessionByUsername(message.Sender.Name);
        var receiverChatUser = _sessions.GetSessionByUsername(message.Receiver.Name);

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
