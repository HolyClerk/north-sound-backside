using Microsoft.EntityFrameworkCore;
using NorthSound.Domain.Entities;

namespace NorthSound.Backend.Infrastructure.Data;

public class SongContext : DbContext
{
	public SongContext(DbContextOptions<SongContext> options)
		: base(options)
	{
		Database.EnsureCreated();
	}

    public DbSet<Song> Songs { get; set; }
}
