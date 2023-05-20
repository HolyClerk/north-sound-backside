using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using System.Security.Claims;

namespace NorthSound.Backend.Services.Abstractions;

public interface IDialogueService
{
    Task<MessageResponse> PrepareMessageAsync(MessageRequest request);
    Task<GenericResponse<ChatUser>> AddChatUserAsync(ClaimsPrincipal userClaims, string connectionId);
    bool RemoveChatUser(string connectionId);
}