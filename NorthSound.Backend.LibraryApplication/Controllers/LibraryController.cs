﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthSound.Backend.Domain.Entities;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.LibraryApplication.ViewModels;
using NorthSound.Backend.Services.Abstractions;

namespace NorthSound.Backend.LibraryApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibraryController : ControllerBase
{
    private readonly ILogger<LibraryController> _logger;
    private readonly ILibraryService _library;

    public LibraryController(
        ILogger<LibraryController> logger,
        ILibraryService libraryService)
    {
        _logger = logger;
        _library = libraryService;
    }

    // GET: api/library/
    [HttpGet]
    public IEnumerable<Song> Get()
    {
        return _library.GetSongs();
    }

    // GET: api/library/5
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var response = await _library.GetSongFileAsync(id);

        if (response.Status is not ResponseStatus.Success)
            return BadRequest();

        return File(
            response.ResponseData.FileStream, 
            response.ResponseData.ContentType, 
            response.ResponseData.Name);
    }

    // POST: api/library/
    [HttpPost]
    public async Task<ActionResult> Post(
        [FromForm] SongViewModel viewModel, 
        [FromServices] IStorageGenerator storage)
    {
        Song mappedEntity = viewModel.MapToSong();

        // На основе данных из вьюмодели (файла) создается и
        // открывается поток для чтения, для будущей записи на хранилище
        // и в БД.
        Stream stream = viewModel.SongFile.OpenReadStream();
        var response = await _library.CreateSongAsync(mappedEntity, stream, storage);

        if (response.Status is not ResponseStatus.Success)
            return BadRequest();

        return Ok(response.ResponseData);
    }

    // DELETE: api/library/5
    [HttpDelete]
    public async Task<ActionResult> Delete(int id)
    {
        var isDeleted = await _library.TryDeleteAsync(id);

        if (isDeleted is false)
            return BadRequest();

        return Ok();
    }
}
