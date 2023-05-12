using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.Responses;

namespace NorthSound.Backend.Domain.POCO.Chat;

public class MessageResponse
{
    public ChatUser Sender { get; set; }

    public ChatUser Receiver { get; set; }

    public Message Message { get; set; }

    public string Details { get; set; }

    public ResponseStatus Status { get; set; }

    public static MessageResponse Success(ChatUser sender, ChatUser receiver, Message message)
    {
        return new MessageResponse
        {
            Sender = sender,
            Receiver = receiver,
            Message = message,
            Status = ResponseStatus.Success
        };
    }

    public static MessageResponse Failed(string details)
    {
        return new MessageResponse
        {
            Details = details,
            Status = ResponseStatus.Failed
        };
    }
}
