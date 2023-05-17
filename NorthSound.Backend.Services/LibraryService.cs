using NorthSound.Backend.DAL.Abstractions;
using NorthSound.Backend.Domain.SongEntities;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;
using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.Services;

/// <summary>
/// Этот сервис работает напрямую с хранилищем аудио файлов и БД.
/// LS предоставляет функционал для получения, удаления и создания
/// новых энтитей в БД и привязке этих записей к реальным файлам, 
/// абстрагируя клиента от внутренней работы кухни.
/// </summary>
public class LibraryService : ILibraryService
{
    private readonly IAsyncSongRepository _repository;

    public LibraryService(IAsyncSongRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<SongDTO> GetSongs() 
    {
        var songs = _repository.GetSongs();
        var songModels = new List<SongDTO>();

        foreach (var song in songs)
        {
            songModels.Add(new SongDTO(song));
        }

        return songModels;
    }

    public async Task<GenericResponse<SongDTO>> GetSongAsync(int id)
    {
        var entity = await _repository.GetSongAsync(id);

        if (entity is null)
            return GenericResponse<SongDTO>.Failed("Не найдено", ResponseStatus.NotFound);

        return GenericResponse<SongDTO>.Success(new SongDTO(entity)); 
    }

    public async Task<GenericResponse<SongFileDTO>> GetSongFileAsync(int id)
    {
        var entity = await _repository.GetSongAsync(id);

        if (entity is null)
            return GenericResponse<SongFileDTO>.Failed("Не найдено", ResponseStatus.NotFound);

        var data = new SongFileDTO()
        {
            Name = $"{entity.Author} - {entity.Name}",
            FileStream = new FileStream(entity.Path.AbsolutePath, FileMode.Open),
            ContentType = ILibraryService.AudioContentType,
        };

        return GenericResponse<SongFileDTO>.Success(data);
    }

    public async Task<GenericResponse<SongDTO>> CreateSongAsync(
        Song entity, 
        Stream stream, 
        IStorageGenerator storage)
    {
        // Случайный путь к файлу
        var pathToFile = storage.GetNewGeneratedPath();
        entity.Path = new Uri(pathToFile);
        // Запускаем задачу на копирование файла (открытого потока) в хранилище
        Task copyTask = CopyStreamToFileAsync(stream, pathToFile);  

        try
        {
            await _repository.CreateAsync(entity);
            await _repository.SaveAsync();
            await copyTask;

            return GenericResponse<SongDTO>.Success(new SongDTO(entity));
        }
        catch (Exception)
        {
            return GenericResponse<SongDTO>.Failed("Неизвестная ошибка");
        }
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

    private static async Task CopyStreamToFileAsync(Stream stream, string pathToCopy)
    {
        using (var fileStream = new FileStream(pathToCopy, FileMode.Create))
        {
            await stream.CopyToAsync(fileStream);
            await stream.DisposeAsync();
        }
    }
}