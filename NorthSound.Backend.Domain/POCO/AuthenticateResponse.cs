using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.Domain.Responses;

public class AuthenticateResponse
{
    public AuthenticateResponse(UserDTO userEntity, string token)
    {
        UserEntity = userEntity;
        Token = token;
    }

    public UserDTO UserEntity { get; set; }

    public string Token { get; set; }
}
