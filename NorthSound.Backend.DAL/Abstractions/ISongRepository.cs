using NorthSound.Backend.Domain.SongEntities;

namespace NorthSound.Backend.DAL.Abstractions;

public interface ISongRepository : IDisposable
{
    IEnumerable<SongDTO> GetSongs();
    SongDTO GetSong(int id);
    void Create(SongDTO entity);
    void Update(SongDTO entity);
    void Delete(int id);
    void Save();
}
