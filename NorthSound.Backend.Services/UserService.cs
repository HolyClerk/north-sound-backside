using NorthSound.Backend.DAL.Abstractions;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;

namespace NorthSound.Backend.Services;

public class UserService : IUserService
{
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserRepository _repository;

    public UserService(IUserRepository userRepository, ITokenHandler tokenHandler)
    {
        _repository = userRepository;
        _tokenHandler = tokenHandler;
    }

    public async Task<BaseResponse<string>> AuthenticateAsync(string username, string password)
    {
        var response = new BaseResponse<string>();
        User? user = await _repository.GetUserAsync(username, password);

        if (user is null)
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
