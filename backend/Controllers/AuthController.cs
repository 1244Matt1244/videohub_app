using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            // Ovdje bi inače provjeravao korisnika iz baze

            // Simulacija autentikacije
            if (req.Email == "test@test.com" && req.Password == "password")
            {
                var token = _jwtService.GenerateToken("123", req.Email);
                return Ok(new { token });
            }

            return Unauthorized("Neispravan email ili lozinka.");
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
