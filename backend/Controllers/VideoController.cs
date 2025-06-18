[Authorize]
[ApiController]
[Route("api/videos")]
public class VideoController : ControllerBase {
    [HttpPost] public IActionResult Upload(VideoUploadRequest r) => /* Mux upload */;
    [HttpGet] public IActionResult List() => /* dobavi video listu */;
    [HttpPost("{id}/purchase")] public IActionResult Purchase(string id) => /* Stripe session */;
}
