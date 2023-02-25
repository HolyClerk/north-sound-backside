using Microsoft.AspNetCore.Mvc;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.Responses;

namespace NorthSound.Backend.Services.Abstractions;

public interface ILibraryService
{
    public const int MaxSizeOfFile = 20971520; // 20 MB
    public const int MinSizeOfFile = 20971;
    public const string AudioContentType = "audio/mpeg";

    IEnumerable<Song> GetSongs();
    Task<Song?> GetSongAsync(int id);
    Task<BaseResponse<FileStreamResult>> GetSongStreamResultAsync(int id);
    Task<Song> CreateSongAsync(Song entity, Stream stream);
    Task<bool> DeleteAsync(int id);
}