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

    public async Task<GenericResponse<string>> AuthenticateAsync(string username, string password)
    {
        var response = new GenericResponse<string>();
        User? user = await _repository.GetUserByNameAsync(username);

        if (user is null || user.Password != password)
        {
            response.Status = ResponseStatus.NotFound; 
            return response;
        }

        var token = _tokenHandler.GenerateToken(user);

        response.Status = ResponseStatus.Success;
        response.ResponseData = token;
        return response;
    }
}
