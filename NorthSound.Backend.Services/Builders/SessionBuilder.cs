using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;

namespace NorthSound.Backend.Services.Builders;

public class SessionBuilder
{
    private readonly Session _session;

    public SessionBuilder()
    {
        _session = new Session();
    }

    public SessionBuilder SetUser(User user)
    {
        _session.CurrentUser = user;
        return this;
    }

    public SessionBuilder CreateConnection(string connectionId)
    {
        _session.Connection = new ChatConnection
        {
            CreatedAt = DateTime.Now,
            Id = connectionId,
        };

        return this;
    }

    public Session Build()
    {
        return _session;
    }
}