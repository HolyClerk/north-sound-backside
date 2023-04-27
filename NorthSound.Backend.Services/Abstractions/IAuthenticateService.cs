using NorthSound.Backend.Domain.Responses;

namespace NorthSound.Backend.Services.Abstractions;

public interface IAuthenticateService
{
    Task<GenericResponse<AuthenticateResponse>> AuthenticateAsync(string username, string password);
}
