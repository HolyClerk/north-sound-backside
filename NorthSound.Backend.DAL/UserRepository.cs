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

    public async Task<User?> GetByNameAsync(string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(user => user.Name == username);

        return user;
    }

    public IEnumerable<User> Get()
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

    public async Task<User?> GetById(int id)
        => await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
}
