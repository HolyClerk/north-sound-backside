using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.Domain.POCO.Chat;

public class ChatUser
{
    public ChatUser() { }

    public ChatUser(User user)
    {
        CurrentUser = user;
    }

    public User CurrentUser { get; set; } = default!;
    
    public ChatConnection Connection { get; set; } = default!;
}
