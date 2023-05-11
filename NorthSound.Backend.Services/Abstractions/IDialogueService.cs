using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using System.Security.Claims;

namespace NorthSound.Backend.Services.Abstractions;

public interface IDialogueService
{
    Task<GenericResponse<Message>> PrepareMessageForSendingAsync(MessageViewModel model, string senderConnectionId);
    Task<GenericResponse<ChatUser>> AddChatUserAsync(ClaimsPrincipal userClaims, string connectionId);
    void RemoveChatUser(string connectionId);
}