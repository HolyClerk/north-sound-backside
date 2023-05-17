using System.ComponentModel.DataAnnotations;
using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.Domain.SongEntities;

public class SongDTO
{
    public SongDTO(Song song)
    {
        Id = song.Id;
        Name = song.Name;
        Author = song.Author;
    }

    public int Id { get; set; } = default;
    public string Name { get; set; } = default!;
    public string Author { get; set; } = default!;
}
