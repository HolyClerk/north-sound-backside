using Microsoft.EntityFrameworkCore;
using NorthSound.Backend.DAL.Abstractions;
using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.DAL;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public User CreateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public User DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserAsync(string username, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(user => (user.Name == username) && (user.Password == password));

        return user;
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
