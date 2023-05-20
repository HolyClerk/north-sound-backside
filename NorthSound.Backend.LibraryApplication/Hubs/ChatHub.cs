using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;

namespace NorthSound.Backend.LibraryApplication.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IDialogueService _dialogueService;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(
        IDialogueService dialogueService,
        ILogger<ChatHub> logger)
    {
        _dialogueService = dialogueService;
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

        var response = await _dialogueService.AddChatUserAsync(userClaims, connectionId);

        if (response.Status is not ResponseStatus.Success)
        {
            _logger.LogInformation("Пользователь не добавлен: {}", response.Details);
            return;
        }

        _logger.LogInformation("Пользователь добавлен в чат: {}", response.Data!.CurrentUser.Id);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _dialogueService.RemoveChatUser(Context.ConnectionId);
        _logger.LogInformation("Пользователь отключен {}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(MessageViewModel viewModel)
    {
        _logger.LogInformation("Попытка отправки сообщения: {} \n\tПолучатель: {}", viewModel.Message, viewModel.ReceiverUsername);

        var request = new MessageRequest(viewModel.ReceiverUsername, Context.ConnectionId, viewModel.Message);
        var response = await _dialogueService.PrepareMessageAsync(request);

        if (response.Status is not ResponseStatus.Success)
        {
            _logger.LogError("Сообщение сформировано неудачно: \n\t{};", response.Details);
            return;
        }

        _logger.LogInformation("Сообщение сформировано удачно {}", response.Message.Value);
        
        await Clients
            .Client(response.Receiver.Connection.Id)
            .SendAsync("ReceiveMessage", response.Sender.CurrentUser.Name, response.Message.Value);
    }
}

public interface IChatClient
{
    Task ReceiveMessage(string username, string message);
}