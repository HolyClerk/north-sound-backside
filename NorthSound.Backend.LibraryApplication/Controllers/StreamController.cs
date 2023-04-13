using Microsoft.AspNetCore.Mvc;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;

namespace NorthSound.Backend.LibraryApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StreamController : ControllerBase
{
    private readonly ILibraryService _library;

    public StreamController(ILibraryService library)
    {
        _library = library;
    }

    // GET: api/library/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FileStream>> Get(int id)
    {
        var response = await _library.GetSongFileAsync(id);

        if (response.Status is not ResponseStatus.Success)
            return BadRequest();

        return File(
            response.Data.FileStream,
            "audio/mpeg");
    }
}
