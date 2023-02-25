using Microsoft.AspNetCore.Mvc;
using NorthSound.Backend.DAL.Abstractions;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;
using System.Security.Cryptography;

namespace NorthSound.Backend.Services;

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

    public async Task<BaseResponse<FileStreamResult>> GetSongStreamResultAsync(int id)
    {
        var response = new BaseResponse<FileStreamResult>();
        var entity = await _repository.GetSongAsync(id);

        if (entity is null)
        {
            response.Status = ResponseStatus.NotFound;
            return response;
        }

        if (entity.Path is null)
            throw new NullReferenceException(nameof(entity.Path));

        var fileStream = new FileStream(entity.Path.AbsolutePath, FileMode.Open);
        var streamResult = new FileStreamResult(fileStream, ILibraryService.AudioContentType)
        {
            FileDownloadName = $"{entity.Author} - {entity.Name}.mp3"
        };

        response.Status = ResponseStatus.Success;
        response.ResponseData = streamResult;

        return response;
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

    public async Task<bool> TryDeleteAsync(int id)
    {
        Song? entity = await _repository.GetSongAsync(id);

        if (entity is null)
            return false;

        await _repository.DeleteAsync(id);
        await _repository.SaveAsync();
        return true;
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