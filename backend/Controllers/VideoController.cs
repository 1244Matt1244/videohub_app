using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using videohub_app.backend.Interfaces; 

[ApiController]
[Route("api/[controller]")]
public class VideoController : ControllerBase
{
    private readonly IMuxService _muxService;
    private readonly IVideoService _videoService;

    public VideoController(IMuxService muxService, IVideoService videoService)
    {
        _muxService = muxService;
        _videoService = videoService;
    }
}

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/videos")]
    public class VideoController : ControllerBase
    {
        private readonly MuxService _muxService;
        private readonly VideoService _videoService;

        public VideoController(MuxService muxService, VideoService videoService)
        {
            _muxService = muxService;
            _videoService = videoService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] VideoUploadRequest request)
        {
            var muxAsset = await _muxService.UploadVideoByUrlAsync(request.Url); // Pretpostavimo da postoji metoda
            var video = new Video
            {
                Id = muxAsset.Id,
                Title = request.Title,
                PlaybackUrl = $"https://stream.mux.com/{muxAsset.PlaybackIds[0].Id}.m3u8",
                Description = request.Description
            };

            await _videoService.SaveVideoAsync(video);

            return Ok(video);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var videos = await _videoService.GetAllVideosAsync();
            return Ok(videos);
        }

        [HttpPost("{id}/purchase")]
        public async Task<IActionResult> Purchase(string id)
        {
            // Simulacija kupnje ili poziv StripeService
            return Ok($"Kupnja videa ID: {id} (stub)");
        }
    }

    public class VideoUploadRequest
    {
        public required string Title { get; set; }
        public required string Url { get; set; }
        public string? Description { get; set; }
    }
}
