using NorthSound.Backend.DAL.Abstractions;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;

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

    public async Task<ResponseBase<SongFile>> GetSongFileAsync(int id)
    {
        var response = new ResponseBase<SongFile>();
        var entity = await _repository.GetSongAsync(id);

        if (entity is null)
        {
            response.Status = ResponseStatus.NotFound;
            return response;
        }

        response.Status = ResponseStatus.Success;
        response.ResponseData = new SongFile()
        {
            Name = $"{entity.Author} - {entity.Name}",
            FileStream = new FileStream(entity.Path.AbsolutePath, FileMode.Open),
            ContentType = ILibraryService.AudioContentType,
        };

        return response;
    }

    public async Task<ResponseBase<SongModel>> CreateSongAsync(Song entity, Stream stream, IStorageGenerator storage)
    {
        var response = new ResponseBase<SongModel>();
        var pathToFile = storage.GenerateStoragePath();

        entity.Path = new Uri(pathToFile);
        Task copyTask = CopyStreamAsync(stream, pathToFile);  // Копируем трек в хранилище

        try
        {
            await _repository.CreateAsync(entity);
            await _repository.SaveAsync();
            await copyTask;

            response.Status = ResponseStatus.Success;
            response.ResponseData = new SongModel(entity);

            return response;
        }
        catch (Exception)
        {
            response.Status = ResponseStatus.Failed;
            return response;
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

    private async Task CopyStreamAsync(Stream stream, string pathToCopy)
    {
        using (var fileStream = new FileStream(pathToCopy, FileMode.Create))
        {
            await stream.CopyToAsync(fileStream);
            await stream.DisposeAsync();
        }
    }
}