using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoApp.Commands.Videos;

namespace VideoApp.Controllers;

[ApiController]
[Route("api/webhooks/mux")]
[AllowAnonymous]
public class MuxWebhookController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MuxWebhookController> _logger;

    public MuxWebhookController(IMediator mediator, ILogger<MuxWebhookController> logger)
    {
        _mediator = mediator; _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        using var reader = new StreamReader(Request.Body);
        var json = await reader.ReadToEndAsync();
        var doc = JsonDocument.Parse(json);
        var eventType = doc.RootElement.GetProperty("type").GetString();
        _logger.LogInformation("MUX webhook: {Type}", eventType);

        if (eventType == "video.asset.ready")
        {
            var data = doc.RootElement.GetProperty("data");
            var assetId = data.GetProperty("id").GetString();
            var playbackId = data.GetProperty("playback_ids")[0].GetProperty("id").GetString();
            var uploadId = data.TryGetProperty("upload_id", out var u) ? u.GetString() : null;

            if (uploadId is not null)
                await _mediator.Send(new MarkVideoReadyCommand(uploadId, assetId!, playbackId!));
        }
        else if (eventType == "video.asset.errored")
        {
            var assetId = doc.RootElement.GetProperty("data").GetProperty("id").GetString();
            await _mediator.Send(new MarkVideoFailedCommand(assetId!));
        }

        return Ok();
    }
}
