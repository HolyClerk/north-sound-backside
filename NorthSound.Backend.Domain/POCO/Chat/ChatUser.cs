using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.Domain.POCO.Chat;

public class ChatUser
{
    public ChatUser() { }

    public ChatUser(UserDTO user)
    {
        CurrentUser = user;
    }

    public UserDTO CurrentUser { get; set; } = default!;
    
    public ChatConnection Connection { get; set; } = default!;
}
