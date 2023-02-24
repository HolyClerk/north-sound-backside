using NorthSound.Backend.LibraryApplication.ViewModels;
using NorthSound.Domain.Entities;

namespace NorthSound.Backend.LibraryApplication.Services.Base;

public interface ILibraryService
{
    public const int MaxSizeOfFile = 20971520; // 20 MB
    public const int MinSizeOfFile = 20971;
    public const string AudioContentType = "audio/mpeg";

    IEnumerable<Song> GetSongs();
    Task<Song?> GetSongAsync(int id);
    FileStream GetFileStream(Song entity);
    Task<Song> CreateSongAsync(SongViewModel song);
    Task DeleteAsync(int id);
}