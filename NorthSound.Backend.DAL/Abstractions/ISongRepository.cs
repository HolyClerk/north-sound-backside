using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.DAL.Abstractions;

public interface ISongRepository : IDisposable
{
    IEnumerable<Song> GetSongs();
    Song GetSong(int id);
    void Create(Song entity);
    void Update(Song entity);
    void Delete(int id);
    void Save();
}
