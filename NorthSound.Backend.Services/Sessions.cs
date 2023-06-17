using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Services.Abstractions;
using NorthSound.Backend.Services.Builders;

namespace NorthSound.Backend.Services;

public class Sessions : ISessions
{
    private readonly List<Session> _sessions;

    public Sessions()
    {
        _sessions = new();
    }

    public Session? AddSession(User user, string connectionId)
    {
        if (IsUserConnected(user))
            return null;

        var builder = new SessionBuilder();

        var session = builder.SetUser(user)
            .CreateConnection(connectionId)
            .Build();

        _sessions.Add(session);
        return session;
    }

    public bool RemoveSession(string connectionId)
    {
        var findedUser = _sessions.FirstOrDefault(u => u.Connection.Id == connectionId);

        if (findedUser is null)
            return false;

        return _sessions.Remove(findedUser);
    }

    public bool IsUserConnected(User user)
        => _sessions.Any(x => x.CurrentUser.Id == user.Id);

    public Session? GetSessionByUsername(string username)
        => _sessions.FirstOrDefault(x => x.CurrentUser.Name == username);

    public Session? GetSession(string connectionId)
        => _sessions.FirstOrDefault(x => x.Connection.Id == connectionId);

    public IEnumerable<Session> GetAllSessions()
        => _sessions;
}
