using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using System.Security.Claims;

namespace NorthSound.Backend.Services.Abstractions;

public interface IChatService
{
    Task<MessageResponse> BuildMessageAsync(MessageRequest request);
    Task<GenericResponse<Session>> CreateSessionAsync(ClaimsPrincipal userClaims, string connectionId);
    bool RemoveSession(string connectionId);
    IEnumerable<Session> GetSessions();
    GenericResponse<Session> GetSession(string connectionId);
}