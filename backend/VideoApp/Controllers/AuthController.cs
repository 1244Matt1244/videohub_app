using MediatR;
using Microsoft.AspNetCore.Mvc;
using VideoApp.Commands.Auth;

namespace VideoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Success ? Ok(new { token = result.Token, user = result.User })
            : BadRequest(new { error = result.Error });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Success ? Ok(new { token = result.Token, user = result.User })
            : Unauthorized(new { error = result.Error });
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Success ? Ok(new { token = result.Token, user = result.User })
            : Unauthorized(new { error = result.Error });
    }
}
