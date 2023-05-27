using NorthSound.Backend.Domain.Responses;

namespace NorthSound.Backend.Domain.POCO.Chat;

public class MessageResponse
{
    public Session Sender { get; set; }

    public Session Receiver { get; set; }

    public MessageDTO Message { get; set; }

    public string Details { get; set; }

    public ResponseStatus Status { get; set; }

    public static MessageResponse Success(Session sender, Session receiver, MessageDTO message)
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
