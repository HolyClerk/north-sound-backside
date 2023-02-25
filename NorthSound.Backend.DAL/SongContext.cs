using Microsoft.EntityFrameworkCore;
using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.DAL;

public class SongContext : DbContext
{
    public SongContext(DbContextOptions<SongContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Song> Songs { get; set; }
}
