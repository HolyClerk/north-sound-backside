using NorthSound.Backend.Domain.POCO.Auth;
using System.ComponentModel.DataAnnotations;

namespace NorthSound.Backend.Domain.ViewModels;

public class AuthenticateViewModel
{
    [Required] public string Username { get; set; }
    [Required] public string Password { get; set; }

    public AuthenticateRequest MapToRequest()
    {
        return new AuthenticateRequest
        {
            Username = Username,
            Password = Password,
        };
    }
}
