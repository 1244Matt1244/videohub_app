using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            // TODO: generate JWT
            return Ok("JWT generated (stub)");
        }

        [HttpPost("google")]
        public IActionResult GoogleLogin([FromBody] GoogleAuthRequest req)
        {
            // TODO: handle Google OAuth
            return Ok("Google login success (stub)");
        }
    }

    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class GoogleAuthRequest
    {
        public required string Token { get; set; }
    }
}
