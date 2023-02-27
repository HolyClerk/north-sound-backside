using Microsoft.IdentityModel.Tokens;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NorthSound.Backend.Infrastructure;

public class JwtTokenGenerator : ITokenHandler
{
    private const int TokenLifetimeInMinutes = 120; // 2 часа

    public const string Issuer = "https://localhost:7099";
    public const string Audience = "https://localhost:7099";

    public SymmetricSecurityKey SecurityKey
    {
        get
        {
            byte[] key = Encoding.UTF8.GetBytes(GetSecretKey());
            return new SymmetricSecurityKey(key);
        }
    }

    /// <summary>
    /// Генерирует и возвращает JWT-токен на основе модели пользователя
    /// </summary>
    /// <returns>JWT токен</returns>
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Name),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };

        
        var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:             Issuer, 
            audience:           Audience, 
            claims:             claims,
            notBefore:          DateTime.Now,
            expires:            DateTime.Now.AddMinutes(TokenLifetimeInMinutes),
            signingCredentials: credentials
        );

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }

    private string GetSecretKey()
        => "my_test_secret_key";
}
