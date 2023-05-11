using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using System.Security.Claims;

namespace NorthSound.Backend.Services.Abstractions;

public interface IDialogueService
{
    GenericResponse<Message> PrepareMessageForSending(MessageViewModel model, string senderConnectionId);
    Task<GenericResponse<ChatUser>> AddChatUser(ClaimsPrincipal userClaims, string connectionId);
    void RemoveChatUser(string connectionId);
}