using Microsoft.AspNetCore.Mvc;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services;
using NorthSound.Backend.Services.Abstractions;

namespace NorthSound.Backend.LibraryApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthorizationController(
        IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/authorization/
    [HttpGet]
    public async Task<ActionResult> Get(string username, string password)
    {
        var response = await _userService.AuthenticateAsync(username, password);

        if (response.Status is not ResponseStatus.Success)
            return ValidationProblem();

        return Ok(response.ResponseData);
    }
}
