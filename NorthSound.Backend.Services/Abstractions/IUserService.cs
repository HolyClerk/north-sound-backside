using NorthSound.Backend.Domain.Responses;

namespace NorthSound.Backend.Services.Abstractions;

public interface IUserService
{
    Task<ResponseBase<string>> AuthenticateAsync(string username, string password);
}
