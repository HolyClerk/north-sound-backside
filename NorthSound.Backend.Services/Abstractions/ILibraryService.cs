﻿using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.Responses;

namespace NorthSound.Backend.Services.Abstractions;

public interface ILibraryService
{
    /// <summary>
    /// Максимально возможный размер загружаемого файла.
    /// </summary>
    public const int MaxSizeOfFile = 20971520; // 20 MB

    /// <summary>
    /// Минимально возможный размер загружаемого файла.
    /// </summary>
    public const int MinSizeOfFile = 20971;

    /// <summary>
    /// Тип ContentType
    /// </summary>
    public const string AudioContentType = "audio/mpeg";

    /// <summary>
    /// Возвращает коллекцию сущностей <see cref="SongModel"/> из базы данных.
    /// </summary>
    /// <returns>Коллекция <see cref="SongModel"/></returns>
    IEnumerable<SongModel> GetSongs();

    /// <summary>
    /// Генерирует <see cref="GenericResponse{SongModel}"/> с содержимым типа <see cref="SongModel"/>, на основе полученной модели из базы данных.
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    /// <returns><see cref="GenericResponse{SongModel}"/></returns>
    Task<GenericResponse<SongModel>> GetSongAsync(int id);

    /// <summary>
    /// Генерирует <see cref="GenericResponse{SongFile}"/> с содержимым типа <see cref="FileStream"/>.
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    /// <returns><see cref="GenericResponse{SongFile}"/></returns>
    Task<GenericResponse<SongFile>> GetSongFileAsync(int id);

    /// <summary>
    /// На основе передаваемого параметра <see cref="Stream"/> создается физический файл в хранилище, а его расположение записывается в модель 
    /// <see cref="Song"/> для размещения сущности в базе данных.
    /// </summary>
    /// <returns><see cref="GenericResponse{SongFile}"/></returns>
    Task<GenericResponse<SongModel>> CreateSongAsync(Song entity, Stream stream, IStorageGenerator storage);

    /// <summary>
    /// Асинхронное удаление записи <see cref="Song"/> из базы данных на основе параметра id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> TryDeleteAsync(int id);
}