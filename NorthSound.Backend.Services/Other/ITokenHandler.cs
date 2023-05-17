using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.Services.Other;

public interface ITokenHandler
{
    string GenerateToken(User user);
}
