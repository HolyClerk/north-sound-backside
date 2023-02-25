using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.DAL.Abstractions;

public interface IAsyncSongRepository : IDisposable
{
    IEnumerable<Song> GetSongs();
    Task<Song?> GetSongAsync(int id);
    Task CreateAsync(Song entity);
    Task UpdateAsync(Song entity);
    Task DeleteAsync(int id);
    Task SaveAsync();
}
