using NorthSound.Backend.Services.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace NorthSound.Backend.LibraryApplication.ViewModels;

public class SongViewModel
{
    [Required] public string Author { get; set; } = null!;

    [Required] public string Name { get; set; } = null!;

    [Required] public IFormFile SongFile { get; set; } = null!;

    public bool Validate()
    {
        return (this is not null) &&
            (this.SongFile.Length < ILibraryService.MaxSizeOfFile) &&
            (this.SongFile.Length > ILibraryService.MinSizeOfFile) &&
            (this.SongFile.ContentType is ILibraryService.AudioContentType);
    }
}
