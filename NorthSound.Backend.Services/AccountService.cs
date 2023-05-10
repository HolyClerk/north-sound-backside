using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NorthSound.Backend.DAL;
using NorthSound.Backend.DAL.Abstractions;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;

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
        var findedUser = await _context.Users.FirstOrDefaultAsync(x => x.Name == request.Username);

        if (findedUser is not null)
            return new GenericResponse<AuthenticateResponse>().Failed("Такой пользователь уже существует");

        if (request.Password.Length < 6)
            return new GenericResponse<AuthenticateResponse>().Failed("Пароль должен содержать не менее 6 символов");

        var newUser = request.MapToUser();
        var token = _tokenHandler.GenerateToken(newUser);

        await CreateUserAsync(newUser);

        return new GenericResponse<AuthenticateResponse>()
        {
            Data = new AuthenticateResponse(newUser, token),
            Status = ResponseStatus.Success
        };
    }

    public async Task<GenericResponse<AuthenticateResponse>> LoginAsync(AuthenticateRequest request)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(x => x.Name == request.Username);

        if ((entity is not User user) || (!user.Password.Equals(request.Password)))
            return new GenericResponse<AuthenticateResponse>().Failed("Такого пользователя не существует");

        var token = _tokenHandler.GenerateToken(user);

        return new GenericResponse<AuthenticateResponse>()
        {
            Data = new AuthenticateResponse(user, token),
            Status = ResponseStatus.Success
        };
    }

    private async Task CreateUserAsync(User newUser)
    {
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }
}
