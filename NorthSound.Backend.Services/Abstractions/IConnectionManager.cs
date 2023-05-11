using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;

namespace NorthSound.Backend.Services.Abstractions;

public interface IConnectionManager
{
    public bool AddUser(UserDTO user, string connectionId);
    public bool RemoveUser(string connectionId);
    ChatUser? GetChatUserByUsername(string username);
    ChatUser? GetChatUserByConnectionId(string connectionId);
}