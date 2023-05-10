using NorthSound.Backend.Domain.SongEntities;

namespace NorthSound.Backend.DAL.Abstractions;

public interface IAsyncSongRepository : IDisposable
{
    IEnumerable<SongDTO> GetSongs();
    Task<SongDTO?> GetSongAsync(int id);
    Task CreateAsync(SongDTO entity);
    Task UpdateAsync(SongDTO entity);
    Task DeleteAsync(int id);
    Task SaveAsync();
}
