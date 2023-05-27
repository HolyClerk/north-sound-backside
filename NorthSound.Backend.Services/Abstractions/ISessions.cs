using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;

namespace NorthSound.Backend.Services.Abstractions;

public interface ISessions
{
    public Session? AddSession(User user, string connectionId);
    public bool RemoveSession(string connectionId);
    IEnumerable<Session> GetAllSessions();   
    Session? GetSessionByUsername(string username);
    Session? GetSession(string connectionId);
}