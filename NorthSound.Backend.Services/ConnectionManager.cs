using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NorthSound.Backend.Services;

public class ConnectionManager : IConnectionManager
{
    private readonly List<ChatUser> _users;

    public ConnectionManager()
    {
        _users = new();
    }

    public ChatUser? AddUser(UserDTO user, string connectionId)
    {
        if (IsChatUserConnected(user))
            return null;

        var chatUser = new ChatUser
        {
            CurrentUser = user,
            Connection = new ChatConnection
            {
                CreatedAt = DateTime.Now,
                Id = connectionId,
            },
        };

        _users.Add(chatUser);
        return chatUser;
    }

    public bool RemoveUser(string connectionId)
    {
        var findedUser = _users.FirstOrDefault(u => u.Connection.Id == connectionId);

        if (findedUser is null)
            return false;

        _users.Remove(findedUser);
        return true;
    }

    public bool IsChatUserConnected(UserDTO user)
        => _users.Any(x => x.CurrentUser.Id == user.Id);

    public ChatUser? GetChatUserByUsername(string connectionId)
        => _users.FirstOrDefault(x => x.Connection.Id == connectionId);

    public ChatUser? GetChatUserByConnectionId(string username)
        => _users.FirstOrDefault(x => x.CurrentUser.Name == username);
}
