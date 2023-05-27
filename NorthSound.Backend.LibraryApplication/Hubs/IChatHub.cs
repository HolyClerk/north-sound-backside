using NorthSound.Backend.Domain.POCO.Chat;

namespace NorthSound.Backend.LibraryApplication.Hubs;

public interface IChatHub
{
    Task ReceiveMessage(string username, string message);
    Task ReceiveClients(IEnumerable<ChatUserDTO> chatUsers);
    Task ReceiveDisconnectedNotification(ChatUserDTO disconnectedUser);
    Task ReceiveConnectectionNotification(ChatUserDTO connectedUser);
}