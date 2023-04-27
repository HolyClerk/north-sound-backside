using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.DAL.Abstractions;

public interface IUserRepository
{
    IEnumerable<User> Get();
    Task<User?> GetByNameAsync(string username);
    Task<User?> GetById(int id);
    User CreateAsync(User entity);
    User UpdateAsync(User entity);
    User DeleteAsync(int id);
    User SaveAsync();
}
