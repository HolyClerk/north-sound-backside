using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.Responses;

namespace NorthSound.Backend.Services.Abstractions;

public interface ILibraryService
{
    public const int MaxSizeOfFile = 20971520; // 20 MB
    public const int MinSizeOfFile = 20971;
    public const string AudioContentType = "audio/mpeg";

    IEnumerable<SongModel> GetSongs();
    Task<GenericResponse<SongModel>> GetSongAsync(int id);
    Task<GenericResponse<SongFile>> GetSongFileAsync(int id);
    Task<GenericResponse<SongModel>> CreateSongAsync(Song entity, Stream stream, IStorageGenerator storage);
    Task<bool> TryDeleteAsync(int id);
}