using NorthSound.Backend.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace NorthSound.Backend.Domain.POCO.Auth;

public class RegisterRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public User MapToUser()
    {
        return new User()
        {
            Name = Username,
            Email = Email,
            Password = Password,
        };
    }
}
