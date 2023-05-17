using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NorthSound.Backend.DAL;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Auth;
using NorthSound.Backend.Services.Other;

namespace NorthSound.Backend.Services;

public class AccountService : IAccountService
{
    private readonly ITokenHandler _tokenHandler;
    private readonly ApplicationContext _context;

    public AccountService(
        ApplicationContext context, 
        ITokenHandler tokenHandler)
    {
        _context = context;
        _tokenHandler = tokenHandler;
    }

    public async Task<GenericResponse<AuthenticateResponse>> RegisterAsync(RegisterRequest request)
    {
        var findedUser = await GetUserByNameAsync(request.Username);

        if (findedUser is not null)
            return GenericResponse<AuthenticateResponse>.Failed("Такой пользователь уже существует");

        if (request.Password.Length < 6)
            return GenericResponse<AuthenticateResponse>.Failed("Пароль должен содержать не менее 6 символов");

        var newUser = request.MapToUser();
        var token = _tokenHandler.GenerateToken(newUser);

        await CreateUserAsync(newUser);

        return GenericResponse<AuthenticateResponse>.Success(new AuthenticateResponse(newUser, token));
    }

    public async Task<GenericResponse<AuthenticateResponse>> LoginAsync(AuthenticateRequest request)
    {
        var entity = await GetUserByNameAsync(request.Username);

        if ((entity is not User user) || (!user.Password.Equals(request.Password)))
            return GenericResponse<AuthenticateResponse>.Failed("Такого пользователя не существует");

        var token = _tokenHandler.GenerateToken(user);

        return GenericResponse<AuthenticateResponse>.Success(new AuthenticateResponse(user, token));
    }
    
    public async Task<User?> GetUserByNameAsync(string username)
        => await _context.Users.FirstOrDefaultAsync(user => user.Name == username);

    private async Task CreateUserAsync(User newUser)
    {
        newUser.CreatedAt = DateTime.UtcNow;
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }
}
