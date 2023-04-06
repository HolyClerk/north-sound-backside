using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.DAL.Abstractions;

public interface IUserRepository
{
    IEnumerable<User> GetUsers();
    Task<User?> GetUserByNameAsync(string username);
    User CreateAsync(User entity);
    User UpdateAsync(User entity);
    User DeleteAsync(int id);
    User SaveAsync();
}
