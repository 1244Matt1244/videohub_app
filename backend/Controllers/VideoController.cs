using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/videos")]
    public class VideoController : ControllerBase
    {
        private static readonly List<Video> videos = new();

        // POST api/videos
        [HttpPost]
        public IActionResult Upload([FromBody] VideoUploadRequest request)
        {
            var video = new Video
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title,
                Description = request.Description,
                PlaybackUrl = request.Url
            };

            videos.Add(video);
            return Ok(video);
        }

        // GET api/videos
        [HttpGet]
        [AllowAnonymous]
        public IActionResult List()
        {
            return Ok(videos);
        }

        // POST api/videos/{id}/purchase
        [HttpPost("{id}/purchase")]
        public IActionResult Purchase(string id)
        {
            var video = videos.Find(v => v.Id == id);
            if (video == null)
                return NotFound($"Video with ID {id} not found.");

            return Ok($"Purchased video with ID: {id}");
        }
    }

    public class VideoUploadRequest
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string Url { get; set; }
    }
}
