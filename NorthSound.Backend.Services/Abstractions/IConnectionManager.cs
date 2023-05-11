using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;

namespace NorthSound.Backend.Services.Abstractions;

public interface IConnectionManager
{
    public ChatUser? AddUser(UserDTO user, string connectionId);
    public bool RemoveUser(string connectionId);
    ChatUser? GetChatUserByUsername(string username);
    ChatUser? GetChatUserByConnectionId(string connectionId);
}