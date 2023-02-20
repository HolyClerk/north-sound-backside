using NorthSound.Domain.Models;

namespace NorthSound.Domain.Interfaces;

public interface ISongRepository : IDisposable
{
    IEnumerable<Song> GetSongs();
    Song GetSong(int id);
    void Create(Song entity);
    void Update(Song entity);
    void Delete(int id);
    void Save();
}
