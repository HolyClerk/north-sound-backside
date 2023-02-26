using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.Services.Abstractions;

public interface ITokenHandler
{
    string GenerateToken(User user);
}
