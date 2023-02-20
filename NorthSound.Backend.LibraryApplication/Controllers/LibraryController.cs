using Microsoft.AspNetCore.Mvc;
using NorthSound.Domain.Models;
using NorthSound.Domain.Interfaces;

namespace NorthSound.Backend.LibraryApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibraryController : ControllerBase
{
    private readonly ILogger<LibraryController> _logger;
    private readonly IAsyncSongRepository _repository;

    public LibraryController(ILogger<LibraryController> logger, IAsyncSongRepository repository)
    {
        _logger = logger;
        _repository = repository;

        _logger.LogInformation("Контроллер запущен");
    }

    // GET: api/songlibrary/
    [HttpGet]
    public IEnumerable<Song> Get()
    {
        return _repository.GetSongs();
    }

    // GET: api/songlibrary/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Song>> Get(int id)
    {
        Song? entity = await _repository.GetSongByIdAsync(id);

        if (entity is Song song)
            return Ok(song);
            
        return BadRequest();
    }

    // PUT: api/songlibrary/
    [HttpPut]
    public async Task<ActionResult<Song>> Put(Song entity)
    {
        if (!ModelState.IsValid || entity is null)
            return BadRequest();

        if (await _repository.GetSongByIdAsync(entity.Id) is null)
            return BadRequest();

        await _repository.Update(entity);
        await _repository.SaveAsync();
        return Ok(entity);
    }

    // POST: api/songlibrary/
    [HttpPost]
    public async Task<ActionResult<Song>> Post(Song entity)
    {
        if (entity is null)
            return BadRequest();

        await _repository.CreateAsync(entity);
        await _repository.SaveAsync();
        return Ok(entity);
    }

    // DELETE: api/songlibrary/5
    [HttpDelete]
    public async Task<ActionResult<Song>> Delete(int id)
    {
        Song? entity = await _repository.GetSongByIdAsync(id);

        if (entity is null)
            return BadRequest();

        await _repository.DeleteAsync(id);
        return Ok(entity);
    }
}
