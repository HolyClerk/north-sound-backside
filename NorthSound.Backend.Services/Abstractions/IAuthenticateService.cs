using NorthSound.Backend.Domain.Responses;

namespace NorthSound.Backend.Services.Abstractions;

public interface IAuthenticateService
{
    Task<GenericResponse<string>> AuthenticateAsync(string username, string password);
}
