using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using System.Security.Claims;

namespace NorthSound.Backend.Services.Abstractions;

public interface IChatService
{
    Task<MessageResponse> BuildMessageAsync(MessageRequest request);
    Task<GenericResponse<ChatUser>> AddChatUserAsync(ClaimsPrincipal userClaims, string connectionId);
    bool RemoveChatUser(string connectionId);
    IEnumerable<ChatUser> GetChatUsers();
    GenericResponse<ChatUser> GetChatUser(string connectionId);
}