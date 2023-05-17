using Microsoft.AspNetCore.Mvc;
using NorthSound.Backend.Domain.SongEntities;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.LibraryApplication.ViewModels;
using NorthSound.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace NorthSound.Backend.LibraryApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibraryController : ControllerBase
{
    private readonly ILibraryService _library;

    public LibraryController(ILibraryService libraryService)
    {
        _library = libraryService;
    }

    [HttpGet]
    public IEnumerable<SongModel> GetAllSongs()
    {
        return _library.GetSongs();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetSongById(int id)
    {
        var response = await _library.GetSongFileAsync(id);
        
        if (response.Status is not ResponseStatus.Success)
            return BadRequest(response);

        return File(
            response.Data!.FileStream,
            response.Data.ContentType,
            response.Data.Name);
    }

    [HttpPost]
    public async Task<ActionResult> PostSong(
        [FromForm] SongViewModel viewModel, 
        [FromServices] IStorageGenerator storage)
    {
        SongDTO mappedEntity = viewModel.MapToSong();

        // На основе данных из вьюмодели (файла) создается и
        // открывается поток для чтения, для будущей записи на хранилище
        // и в БД.
        Stream stream = viewModel.SongFile.OpenReadStream();
        var response = await _library.CreateSongAsync(mappedEntity, stream, storage);

        if (response.Status is not ResponseStatus.Success)
            return BadRequest(response);

        return Ok(response.Data);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteById(int id)
    {
        var isDeleted = await _library.TryDeleteAsync(id);

        if (isDeleted is false)
            return BadRequest();

        return Ok();
    }
}
