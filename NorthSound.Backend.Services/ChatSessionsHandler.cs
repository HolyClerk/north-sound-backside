using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Services.Abstractions;

namespace NorthSound.Backend.Services;

public class ChatSessionsHandler : IChatSessions
{
    private readonly List<ChatUser> _users;

    public ChatSessionsHandler()
    {
        _users = new();
    }

    public ChatUser? AddUser(User user, string connectionId)
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

        return _users.Remove(findedUser);
    }

    public bool IsChatUserConnected(User user)
        => _users.Any(x => x.CurrentUser.Id == user.Id);

    public ChatUser? GetChatUserByUsername(string username)
        => _users.FirstOrDefault(x => x.CurrentUser.Name == username);

    public ChatUser? GetChatUserByConnectionId(string connectionId)
        => _users.FirstOrDefault(x => x.Connection.Id == connectionId);

    public IEnumerable<ChatUser> GetAllConnections()
        => _users;
}
