using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwt;
        public AuthController(JwtService jwt) => _jwt = jwt;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            var token = _jwt.GenerateToken("123", req.Email);
            return Ok(new { token });
        }
    }

    public record LoginRequest(string Email, string Password);
}
