namespace NorthSound.Backend.Domain.POCO.Chat;

public class MessageViewModel
{
    public string ReceiverUsername { get; set; } = default!;
    public string Message { get; set; } = default!;
}
