using Microsoft.AspNetCore.Mvc;
using NorthSound.Backend.Domain.SongEntities;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.LibraryApplication.ViewModels;
using NorthSound.Backend.Services.Abstractions;

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

    // GET: api/library/
    [HttpGet]
    public IEnumerable<SongModel> Get()
    {
        return _library.GetSongs();
    }

    // GET: api/library/5
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var response = await _library.GetSongFileAsync(id);
        
        if (response.Status is not ResponseStatus.Success)
            return BadRequest(response);

        return File(
            response.Data!.FileStream,
            response.Data.ContentType,
            response.Data.Name);
    }

    // POST: api/library/
    [HttpPost]
    public async Task<ActionResult> Post(
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
