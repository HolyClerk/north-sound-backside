using System.ComponentModel.DataAnnotations;

namespace NorthSound.Backend.Domain.Entities;

public class SongModel
{
    public SongModel(Song song)
    {
        Id = song.Id;
        Name = song.Name;
        Author = song.Author;
    }

    public int Id { get; set; } = default;
    public string Name { get; set; } = default!;
    public string Author { get; set; } = default!;
}
