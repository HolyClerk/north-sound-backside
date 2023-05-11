using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;

namespace NorthSound.Backend.LibraryApplication.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IDialogueService _dialogueService;
    private readonly IConnectionManager _connectionManager;
    private readonly IAccountService _accountService;

    public ChatHub(
        IAccountService accountService,
        IConnectionManager chatService,
        IDialogueService dialogueService)
    {
        _connectionManager = chatService;
        _accountService = accountService;
        _dialogueService = dialogueService;
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var usernameClaim = Context.User?.Claims
            .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

        if (usernameClaim is null)
            return;

        if (await _accountService.GetUserByNameAsync(usernameClaim.Value) is UserDTO user)
            _connectionManager.AddUser(user, connectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionManager.RemoveUser(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(MessageViewModel message)
    {
        if (_connectionManager.GetChatUserByConnectionId(Context.ConnectionId) is not ChatUser sender)
            return;

        if (_connectionManager.GetChatUserByUsername(message.ReceiverUsername) is not ChatUser receiver)
            return;

        // TODO: Проверить, создан ли диалог, если нет - создать
        // Отправить сообщение через сервис, для сохранения в БД

        await Clients.Client(receiver.Connection.Id)
            .SendAsync("Receive", sender.CurrentUser.Name, message.Message);
    }
}
