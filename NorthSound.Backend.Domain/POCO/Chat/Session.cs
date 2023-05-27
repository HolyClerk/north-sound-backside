using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.Domain.POCO.Chat;

public class Session
{
    public Session() { }

    public Session(User user)
    {
        CurrentUser = user;
    }

    public User CurrentUser { get; set; } = default!;
    
    public ChatConnection Connection { get; set; } = default!;
}
