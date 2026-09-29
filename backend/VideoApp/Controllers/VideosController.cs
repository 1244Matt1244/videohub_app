using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoApp.Commands.Videos;
using VideoApp.Queries.Videos;

namespace VideoApp.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VideosController : ControllerBase
{
    private readonly IMediator _mediator;
    public VideosController(IMediator mediator) => _mediator = mediator;

    private Guid UserId => Guid.Parse(User.FindFirst("sub")?.Value
        ?? throw new UnauthorizedAccessException());

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool onlyMine = false)
        => Ok(await _mediator.Send(new GetVideosQuery(UserId, onlyMine)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var video = await _mediator.Send(new GetVideoByIdQuery(id, UserId));
        return video is null ? NotFound() : Ok(video);
    }

    [HttpPost("upload-url")]
    public async Task<IActionResult> GetUploadUrl()
        => Ok(await _mediator.Send(new CreateUploadUrlCommand(UserId)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVideoCommand command)
    {
        var result = await _mediator.Send(command with { UserId = UserId });
        return result.Success
            ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data)
            : BadRequest(new { error = result.Error });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVideoCommand command)
    {
        var result = await _mediator.Send(command with { Id = id, UserId = UserId });
        return result.Success ? Ok(result.Data) : NotFound(new { error = result.Error });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteVideoCommand(id, UserId));
        return result.Success ? NoContent() : NotFound(new { error = result.Error });
    }
}
