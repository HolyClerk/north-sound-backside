using Microsoft.EntityFrameworkCore;
using NorthSound.Backend.DAL;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Auth;
using NorthSound.Backend.Services.Other;
using Microsoft.Extensions.Caching.Memory;

namespace NorthSound.Backend.Services;

public class AccountService : IAccountService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ITokenHandler _tokenHandler;
    private readonly ApplicationContext _context;

    public AccountService(
        ApplicationContext context,
        ITokenHandler tokenHandler,
        IMemoryCache memoryCache)
    {
        _context = context;
        _tokenHandler = tokenHandler;
        _memoryCache = memoryCache;
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
        bool shouldCache = false;

        _memoryCache.TryGetValue(request.Username, out User? user);
        
        if (user is null)
        {
            shouldCache = true;
            user = await GetUserByNameAsync(request.Username);
        }

        if (user is null || !user.Password.Equals(request.Password))
            return GenericResponse<AuthenticateResponse>.Failed("Такого пользователя не существует");

        if (shouldCache)
        {
            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            _memoryCache.Set(user.Name, user, options);
        }

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
