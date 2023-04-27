using NorthSound.Backend.DAL.Abstractions;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;

namespace NorthSound.Backend.Services;

public class AuthenticateService : IAuthenticateService
{
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserRepository _repository;

    public AuthenticateService(
        IUserRepository userRepository, 
        ITokenHandler tokenHandler)
    {
        _repository = userRepository;
        _tokenHandler = tokenHandler;
    }

    public async Task<GenericResponse<AuthenticateResponse>> AuthenticateAsync(string username, string password)
    {
        var entity = await _repository.GetByNameAsync(username);

        if (entity is not User user)
            return new GenericResponse<AuthenticateResponse>() { Status = ResponseStatus.NotFound };

        if (!user.Password.Equals(password))
            return new GenericResponse<AuthenticateResponse>() { Status = ResponseStatus.NotFound };

        // todo: get token
        var token = _tokenHandler.GenerateToken(user);

        return new GenericResponse<AuthenticateResponse>()
        {
            Data = new AuthenticateResponse(user, token),
            Status = ResponseStatus.Success
        };
    }
}
