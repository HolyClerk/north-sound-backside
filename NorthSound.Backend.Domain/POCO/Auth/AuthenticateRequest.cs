using System.ComponentModel.DataAnnotations;

namespace NorthSound.Backend.Domain.POCO.Auth;

public class AuthenticateRequest
{
    [Required] public string Username { get; set; }

    [Required] public string Password { get; set; }
}
