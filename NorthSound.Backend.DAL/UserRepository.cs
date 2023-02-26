using NorthSound.Backend.DAL.Abstractions;
using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.DAL;

public class UserRepository : IUserRepository
{
    public User CreateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public User DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetUsers()
    {
        throw new NotImplementedException();
    }

    public User SaveAsync()
    {
        throw new NotImplementedException();
    }

    public User UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }
}
