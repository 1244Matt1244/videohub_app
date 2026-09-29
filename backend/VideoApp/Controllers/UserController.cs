using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoApp.Commands.Users;
using VideoApp.Queries.Users;

namespace VideoApp.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    public UserController(IMediator mediator) => _mediator = mediator;

    private Guid UserId => Guid.Parse(User.FindFirst("sub")?.Value
        ?? throw new UnauthorizedAccessException());

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var profile = await _mediator.Send(new GetUserProfileQuery(UserId));
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        var result = await _mediator.Send(command with { UserId = UserId });
        return result.Success ? Ok(result.Data) : BadRequest(new { error = result.Error });
    }

    [HttpPut("preferences/dark-mode")]
    public async Task<IActionResult> SetDarkMode([FromBody] SetDarkModeRequest request)
    {
        await _mediator.Send(new UpdateDarkModeCommand(UserId, request.IsDark));
        return NoContent();
    }
}

public record SetDarkModeRequest(bool IsDark);
