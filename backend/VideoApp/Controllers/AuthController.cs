using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using VideoApp.Services;

namespace VideoApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwt;

        public AuthController(JwtService jwt)
        {
            _jwt = jwt;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req?.Email))
                return BadRequest("Email is required");

            // Ovdje bi inače išla provjera lozinke i korisnika
            var token = _jwt.GenerateToken("123", req.Email);
            return Ok(new { token });
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/api/auth/google-callback"
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded || result.Principal == null)
                return BadRequest("Google autentikacija nije uspjela.");

            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return BadRequest("E-mail nije pronađen u Google profilu.");

            var token = _jwt.GenerateToken("google_user_id", email);
            return Redirect($"{Request.Scheme}://{Request.Host}/dashboard?token={token}");
        }
    }

    public record LoginRequest(string Email, string Password);
}
