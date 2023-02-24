using NorthSound.Domain.Interfaces;
using NorthSound.Domain.Entities;
using System.Security.Cryptography;

namespace NorthSound.Backend.Infrastructure;

public class LibraryService : ILibraryService
{
    private readonly IAsyncSongRepository _repository;

    public LibraryService(IAsyncSongRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Song> GetSongs() 
    {
        return _repository.GetSongs();
    }

    public async Task<Song?> GetSongAsync(int id)
    {
        return await _repository.GetSongAsync(id); 
    }

    public FileStream GetFileStream(Song entity)
    {
        if (entity.Path is null)
            throw new NullReferenceException(nameof(entity.Path));

        return new FileStream(entity.Path.AbsolutePath, FileMode.Open);
    }

    public async Task<Song> CreateSongAsync(Song entity, Stream stream)
    {
        string generatedName = RandomNumberGenerator
            .GetInt32(-2_147_483_640, 2_147_483_640)
            .ToString();

        var songPath = $"Z:\\Storage\\{generatedName}";     // Путь к треку (внутри хранилища)

        Task copyTask = CopyStreamAsync(stream, songPath);  // Копируем трек в хранилище
        Song mappedSong = MapEntity(entity, songPath);      // Получаем сущность 

        await _repository.CreateAsync(mappedSong);
        await _repository.SaveAsync();
        await copyTask;

        return mappedSong;
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveAsync();
    }

    private async Task CopyStreamAsync(Stream stream, string pathToCopy)
    {
        using (var fileStream = new FileStream(pathToCopy, FileMode.Create))
        {
            await stream.CopyToAsync(fileStream);
            await stream.DisposeAsync();
        }
    }

    private static Song MapEntity(Song entityToMap, string songPath)
    {
        Song mappedEntity = new Song()
        {
            Name = entityToMap.Name,
            Author = entityToMap.Author,
            Path = new Uri(songPath),
        };

        return mappedEntity;
    }
}