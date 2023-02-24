using NorthSound.Backend.LibraryApplication.Services.Base;
using NorthSound.Backend.LibraryApplication.ViewModels;
using NorthSound.Domain.Interfaces;
using NorthSound.Domain.Entities;
using System.Security.Cryptography;

namespace NorthSound.Backend.LibraryApplication.Services;

internal class LibraryService : ILibraryService
{
    private readonly ILogger<LibraryService> _logger;
    private readonly IAsyncSongRepository _repository;

    public LibraryService(
        ILogger<LibraryService> logger, 
        IAsyncSongRepository repository)
    {
        _logger = logger;
        _repository = repository;

        _logger.LogInformation("Контроллер");
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

    public async Task<Song> CreateSongAsync(SongViewModel viewModel)
    {
        string generatedName = RandomNumberGenerator
            .GetInt32(-2_147_483_640, 2_147_483_640)
            .ToString();

        var songPath = $"Z:\\Storage\\{generatedName}";             // Путь к треку (внутри хранилища)

        Task copyTask = CopyAsync(viewModel.SongFile, songPath);    // Копируем трек в хранилище
        Song extractedSong = ExtractEntity(viewModel, songPath);    // Получаем сущность из вьюмодели

        await _repository.CreateAsync(extractedSong);
        await _repository.SaveAsync();
        await copyTask;

        return extractedSong;
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveAsync();
    }

    private async Task CopyAsync(IFormFile fileToCopy, string pathToCopy)
    {
        using (var fileStream = new FileStream(pathToCopy, FileMode.Create))
        {
            await fileToCopy.CopyToAsync(fileStream);
        }
    }

    private static Song ExtractEntity(SongViewModel songViewModel, string songPath)
    {
        Song convertedEntity = new Song()
        {
            Name = songViewModel.Name,
            Author = songViewModel.Author,
            Path = new Uri(songPath),
        };

        return convertedEntity;
    }
}