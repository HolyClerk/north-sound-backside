using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.Domain.POCO.Chat;

public class MessageDTO
{
    public MessageDTO() { }

    public MessageDTO(User sender, User receiver, string messageData)
    {
        Sender = sender;
        Receiver = receiver;
        Value = messageData;
    }

    public User Sender { get; set; }
    public User Receiver { get; set; }

    public string Value { get; set; }
}
