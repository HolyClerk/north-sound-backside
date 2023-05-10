using Microsoft.EntityFrameworkCore;
using NorthSound.Backend.DAL.Abstractions;
using NorthSound.Backend.Domain.SongEntities;

namespace NorthSound.Backend.DAL;

public class AsyncSongRepository : IAsyncSongRepository
{
    private readonly ApplicationContext _songContext;

    public AsyncSongRepository(ApplicationContext songContext)
    {
        _songContext = songContext;
    }

    public IEnumerable<SongDTO> GetSongs()
    {
        return _songContext.Songs;
    }

    public async Task CreateAsync(SongDTO entity)
    {
        await _songContext.AddAsync(entity);
    }

    public async Task<SongDTO?> GetSongAsync(int id)
    {
        return
            await _songContext.Songs.FindAsync(id);
    }

    public async Task UpdateAsync(SongDTO entity)
    {
        SongDTO? current = await _songContext.Songs.FindAsync(entity.Id);

        if (current is not null)
            _songContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task DeleteAsync(int id)
    {
        SongDTO? entity = await _songContext.Songs.FindAsync(id);

        if (entity is SongDTO currentEntity)
            _songContext.Songs.Remove(currentEntity);
    }

    public async Task SaveAsync()
    {
        await _songContext.SaveChangesAsync();
    }

    private bool _isDisposed = false;

    public virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _songContext.Dispose();
            }
        }

        _isDisposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
