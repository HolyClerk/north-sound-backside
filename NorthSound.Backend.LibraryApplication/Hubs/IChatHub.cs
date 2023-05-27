namespace NorthSound.Backend.LibraryApplication.Hubs;

public interface IChatHub
{
    Task ReceiveMessage(string username, string message);
}