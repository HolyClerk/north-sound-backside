using NorthSound.Backend.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace NorthSound.Backend.Domain.POCO.Auth;

public class RegisterRequest
{
    [Required] public string Username { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }

    [Required] public string Password { get; set; }

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
