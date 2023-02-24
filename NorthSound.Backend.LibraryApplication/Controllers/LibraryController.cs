using Microsoft.AspNetCore.Mvc;
using NorthSound.Domain.Entities;
using NorthSound.Backend.LibraryApplication.Services.Base;
using NorthSound.Backend.LibraryApplication.ViewModels;

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

    // GET: api/songlibrary/
    [HttpGet]
    public IEnumerable<Song> Get()
    {
        return _library.GetSongs();
    }

    // GET: api/songlibrary/5
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        Song? entity = await _library.GetSongAsync(id);

        if (entity is null)
            return BadRequest();

        try
        {
            var fileStream = _library.GetFileStream(entity);
            return File(fileStream, ILibraryService.AudioContentType, $"{entity.Author} - {entity.Name}.mp3");
        }
        catch (Exception exception)
        {
            _logger.LogWarning("Ошибка в получении файла: id: {id}", id);
            _logger.LogWarning("Вызвано исключение: {exception}", exception);
            return NoContent();
        }
    }

    // POST: api/songlibrary/
    [HttpPost]
    public async Task<ActionResult> Post([FromForm] SongViewModel viewModel)
    {
        if (viewModel.Validate() is false)
            return BadRequest();

        try
        {
            Song newEntity = await _library.CreateSongAsync(viewModel);
            return Ok(newEntity);
        }
        catch (Exception)
        {
            _logger.LogWarning("Bad request // Невозможно создать сущность в БД");
            return BadRequest();
        }
        
    }

    // DELETE: api/songlibrary/5
    [HttpDelete]
    public async Task<ActionResult> Delete(int id)
    {
        Song? entity = await _library.GetSongAsync(id);

        if (entity is null)
            return BadRequest();

        await _library.DeleteAsync(id);
        return Ok(entity);
    }
}
