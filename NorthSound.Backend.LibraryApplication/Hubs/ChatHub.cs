using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Domain.ViewModels;
using NorthSound.Backend.Services.Abstractions;

namespace NorthSound.Backend.LibraryApplication.Hubs;

[Authorize]
public class ChatHub : Hub<IChatHub>
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(
        IChatService dialogueService,
        ILogger<ChatHub> logger)
    {
        _chatService = dialogueService;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userClaims = Context.User;
        var connectionId = Context.ConnectionId;

        if (userClaims is null)
        {
            _logger.LogError("Claims не найдены");
            return;
        }

        var response = await _chatService.AddChatUserAsync(userClaims, connectionId);

        if (response.Status is not ResponseStatus.Success)
        {
            _logger.LogInformation("Пользователь не добавлен: {}", response.Details);
            return;
        }

        var username = response.Data!.CurrentUser.Name;

        _logger.LogInformation("Пользователь добавлен в чат: {}", username);

        await Clients.All.ReceiveConnectectionNotification(new ChatUserDTO(username));
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var response = _chatService.GetChatUser(Context.ConnectionId);

        if (response.Status is not ResponseStatus.Success)
            return;

        var username = response.Data!.CurrentUser.Name;

        _chatService.RemoveChatUser(Context.ConnectionId);
        _logger.LogInformation("Пользователь отключен {}", username);

        await Clients.All.ReceiveDisconnectedNotification(new ChatUserDTO(username));
        await base.OnDisconnectedAsync(exception);
    }

    public async Task GetAllClients()
    {
        var response = _chatService.GetChatUsers();
        var dtoList = new List<ChatUserDTO>();

        foreach (var chatUser in response)
        {
            dtoList.Add(new ChatUserDTO(chatUser.CurrentUser.Name));
        }

        await Clients.Client(Context.ConnectionId)
            .ReceiveClients(dtoList);
    }

    public async Task SendMessage(MessageViewModel viewModel)
    {
        _logger.LogInformation("Попытка отправки сообщения: {} \n\tПолучатель: {}", viewModel.Message, viewModel.ReceiverUsername);

        var request = new MessageRequest(viewModel.ReceiverUsername, Context.ConnectionId, viewModel.Message);
        var response = await _chatService.BuildMessageAsync(request);

        if (response.Status is not ResponseStatus.Success)
        {
            _logger.LogError("Сообщение сформировано неудачно: \n\t{};", response.Details);
            return;
        }

        _logger.LogInformation("Сообщение сформировано удачно {}", response.Message.Value);
        
        await Clients.Client(response.Receiver.Connection.Id)
            .ReceiveMessage(response.Sender.CurrentUser.Name, response.Message.Value);
    }
}
