namespace NorthSound.Backend.Domain.POCO.Chat;

public class ChatUserDTO
{
    public ChatUserDTO(string userName)
    {
        UserName = userName;
    }

    public string UserName { get; set; }
}
