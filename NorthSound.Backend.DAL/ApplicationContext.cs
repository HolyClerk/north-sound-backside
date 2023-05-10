using Microsoft.EntityFrameworkCore;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.SongEntities;
using System.Collections.Generic;

namespace NorthSound.Backend.DAL;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<SongDTO> Songs { get; set; }
    public DbSet<UserDTO> Users { get; set; }
    public DbSet<MessageDTO> Messages { get; set; }
    public DbSet<DialogueDTO> Dialogues { get; set; }
}
