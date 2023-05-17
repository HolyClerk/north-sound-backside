using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NorthSound.Backend.Domain.POCO.Auth;
using NorthSound.Backend.Domain.POCO.Chat;
using NorthSound.Backend.Domain.Responses;
using NorthSound.Backend.LibraryApplication.Hubs;

namespace NorthSound.Backend.LibraryApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatTestingController : ControllerBase
{
    private readonly ChatHub _hub;

    public ChatTestingController(ChatHub hubContext)
    {
        _hub = hubContext;
    }

    [Authorize]
    [HttpPost("chat")]
    public async Task<ActionResult> Send(MessageViewModel viewModel)
    {
        await _hub.SendMessage(viewModel);
        return Ok("Sended");
    }
}
