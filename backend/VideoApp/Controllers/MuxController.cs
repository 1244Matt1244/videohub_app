using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using VideoApp.Interfaces;

namespace VideoApp.Controllers
{
    [ApiController]
    [Route("api/mux")]
    public class MuxController : ControllerBase
    {
        private readonly IMuxService _muxService;

        public MuxController(IMuxService muxService)
        {
            _muxService = muxService;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> UploadVideo([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Niste uploadali datoteku");

                await using var stream = file.OpenReadStream();
                var assetId = await _muxService.UploadVideoAsync(stream, file.FileName);
                
                return Ok(new 
                { 
                    AssetId = assetId,
                    FileName = file.FileName,
                    Size = file.Length 
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}