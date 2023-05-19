using NorthSound.Backend.Domain.SongEntities;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Services.Other;
using NorthSound.Backend.DAL;
using Microsoft.EntityFrameworkCore;
using System;

namespace NorthSound.Backend.Services;

/// <summary>
/// Этот сервис работает напрямую с хранилищем аудио файлов и БД.
/// LS предоставляет функционал для получения, удаления и создания
/// новых энтитей в БД и привязке этих записей к реальным файлам, 
/// абстрагируя клиента от внутренней работы кухни.
/// </summary>
public class LibraryService : ILibraryService
{
    private readonly ApplicationContext _context;
    private readonly ILocator _locator;

    public LibraryService(
        ApplicationContext context,
        ILocator locator)
    {
        _context = context;
        _locator = locator;
    }

    public IEnumerable<SongDTO> GetSongs() 
    {
        var songs = _context.Songs;
        var songModels = new List<SongDTO>();

        foreach (var song in songs)
        {
            songModels.Add(new SongDTO(song));
        }

        return songModels;
    }

    public async Task<GenericResponse<SongDTO>> GetSongAsync(int id)
    {
        var entity = await _context.Songs.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            return GenericResponse<SongDTO>.Failed("Не найдено", ResponseStatus.NotFound);

        return GenericResponse<SongDTO>.Success(new SongDTO(entity)); 
    }

    public async Task<GenericResponse<SongFileDTO>> GetSongFileAsync(int id)
    {
        var entity = await _context.Songs.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            return GenericResponse<SongFileDTO>.Failed("Не найдено", ResponseStatus.NotFound);

        var data = new SongFileDTO()
        {
            Name = $"{entity.Author} - {entity.Name}",
            FileStream = new FileStream(_locator.GetWorkPath() + entity.FileName, FileMode.Open),
            ContentType = ILibraryService.AudioContentType,
        };

        return GenericResponse<SongFileDTO>.Success(data);
    }

    public async Task<GenericResponse<SongDTO>> CreateSongAsync(Song entity, Stream stream)
    {
        var uri = _locator.GeneratePath();
        entity.FileName = Path.GetFileName(uri.LocalPath);
        Task locateTask = _locator.LocateAsync(stream, uri.AbsolutePath);  

        try
        {
            await _context.Songs.AddAsync(entity);
            await _context.SaveChangesAsync();
            await locateTask;

            return GenericResponse<SongDTO>.Success(new SongDTO(entity));
        }
        catch (Exception)
        {
            return GenericResponse<SongDTO>.Failed("Неизвестная ошибка");
        }
    }

    public async Task<bool> TryDeleteAsync(int id)
    {
        Song? entity = await _context.Songs.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            return false;

        _context.Songs.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}