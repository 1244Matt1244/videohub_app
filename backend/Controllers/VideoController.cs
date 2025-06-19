using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/videos")]
    public class VideoController : ControllerBase
    {
        // POST api/videos
        [HttpPost]
        public IActionResult Upload([FromBody] VideoUploadRequest request)
        {
            // TODO: Implement Mux upload
            return Ok("Video uploaded (stub)");
        }

        // GET api/videos
        [HttpGet]
        public IActionResult List()
        {
            // TODO: Return video list
            return Ok(new[] { "video1", "video2" });
        }

        // POST api/videos/{id}/purchase
        [HttpPost("{id}/purchase")]
        public IActionResult Purchase(string id)
        {
            // TODO: Implement Stripe session
            return Ok($"Purchased video with ID: {id} (stub)");
        }
    }

    // 👇 Dummy DTO - zamijeni sa stvarnim propertiima koje ti trebaju
    public class VideoUploadRequest
    {
        public required string Title { get; set; }
        public required string Url { get; set; }
    }
}
