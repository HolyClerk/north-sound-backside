using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Services.Other;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NorthSound.Backend.Infrastructure;

public class JwtTokenGenerator : ITokenHandler
{
    private readonly IConfiguration _configuration;

    public const int TokenLifetime = 60; 

    public const string Issuer = "https://localhost:7125";
    public const string Audience = "https://localhost:7125";

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Генерирует и возвращает JWT-токен на основе модели пользователя
    /// </summary>
    /// <returns>JWT токен</returns>
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };

        var credentials = new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:             Issuer, 
            audience:           Audience, 
            claims:             claims,
            notBefore:          DateTime.Now,
            expires:            DateTime.Now.AddMinutes(TokenLifetime),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token); 
    }

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        byte[] key = Encoding.ASCII.GetBytes(_configuration["Secret"]!);
        return new SymmetricSecurityKey(key);
    }
}
