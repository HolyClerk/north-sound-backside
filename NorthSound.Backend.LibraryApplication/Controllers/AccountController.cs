using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.Services.Abstractions;

namespace NorthSound.Backend.LibraryApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(
        IAccountService userService)
    {
        _accountService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        if (ModelState.IsValid is false)
            return BadRequest(ModelState);

        var response = await _accountService.RegisterAsync(request);

        if (response.Status is not ResponseStatus.Success)
            return ValidationProblem(detail: response.Details);

        return Ok(response.Data!.Token);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] AuthenticateRequest request)
    {
        if (ModelState.IsValid is false)
            return BadRequest(ModelState);

        var response = await _accountService.LoginAsync(request);

        if (response.Status is not ResponseStatus.Success)
            return ValidationProblem(detail: response.Details);

        return Ok(response.Data!.Token);
    }

    [HttpGet("secret")]
    [Authorize]
    public async Task<ActionResult> GetSecret()
    {
        return Ok("Ok");
    }
}
