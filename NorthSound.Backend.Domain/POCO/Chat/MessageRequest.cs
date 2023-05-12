namespace NorthSound.Backend.Domain.POCO.Chat;

public class MessageRequest
{
    public MessageRequest() { }

    public MessageRequest(string receiverUsername, string senderConnectionId, string message)
    {
        ReceiverUsername = receiverUsername;
        SenderConnectionId = senderConnectionId;
        Message = message;
    }

    public string ReceiverUsername { get; set; } = default!;
    public string SenderConnectionId { get; set; } = default!;
    public string Message { get; set; } = default!;
}
