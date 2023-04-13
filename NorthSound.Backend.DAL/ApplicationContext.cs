using Microsoft.EntityFrameworkCore;
using NorthSound.Backend.Domain.Entities;
using System.Collections.Generic;

namespace NorthSound.Backend.DAL;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        // Database.EnsureCreated();
    }

    public DbSet<Song> Songs { get; set; }
    public DbSet<User> Users { get; set; }
}
