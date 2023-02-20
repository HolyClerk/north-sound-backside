using NorthSound.Domain.Models;

namespace NorthSound.Domain.Interfaces;

public interface IAsyncSongRepository : IDisposable
{
    IEnumerable<Song> GetSongs();
    Task<Song?> GetSongByIdAsync(int id);
    Task CreateAsync(Song entity);
    Task Update(Song entity);
    Task DeleteAsync(int id);
    Task SaveAsync();
}
