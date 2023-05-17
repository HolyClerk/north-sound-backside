using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.POCO.Auth;
using NorthSound.Backend.Domain.Responses;

namespace NorthSound.Backend.Services.Abstractions;

public interface IAccountService
{
    Task<GenericResponse<AuthenticateResponse>> RegisterAsync(RegisterRequest request);
    Task<GenericResponse<AuthenticateResponse>> LoginAsync(AuthenticateRequest request);
    Task<User?> GetUserByNameAsync(string username);
}
