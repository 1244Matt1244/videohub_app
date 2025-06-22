using Microsoft.AspNetCore.Mvc;
using VideoApp.Interfaces;
using VideoApp.Models;

namespace VideoApp.Controllers
{
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

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] VideoUploadRequest request)
        {
            var assetId = await _muxService.UploadVideo(request.VideoFile);
            
            var video = new Video
            {
                Title = request.Title,
                Description = request.Description,
                MuxAssetId = assetId,
                IsPremium = request.IsPremium
            };

            await _videoService.UploadVideo(video);
            return Ok(video);
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var videos = await _videoService.GetAllVideos();
            return Ok(videos);
        }
    }
}