using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/mux")]
    public class MuxController : ControllerBase
    {
        private readonly MuxService _muxService;

        public MuxController(MuxService muxService)
        {
            _muxService = muxService;
        }

        [HttpPost("upload")]
        [Authorize] // Autentifikacija potrebna
        public async Task<IActionResult> UploadVideo()
        {
            try
            {
                var asset = await _muxService.UploadVideoAsync();
                return Ok(new
                {
                    asset.Id,
                    asset.PlaybackIds,
                    asset.Status
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
