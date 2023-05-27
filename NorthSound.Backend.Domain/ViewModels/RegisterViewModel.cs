using NorthSound.Backend.Domain.POCO.Auth;
using System.ComponentModel.DataAnnotations;

namespace NorthSound.Backend.Domain.ViewModels;

public class RegisterViewModel
{
    [Required] public string Username { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }

    [Required] public string Password { get; set; }

    public RegisterRequest MapToRequest()
    {
        return new RegisterRequest
        {
            Username = Username,
            Email = Email,
            Password = Password,
        };
    }
}
