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

        // Standardni login s e-mailom i lozinkom
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req?.Email))
                return BadRequest("Email is required");

            // Ovdje bi inače išla provjera korisnika i lozinke
            var token = _jwt.GenerateToken("123", req.Email);
            return Ok(new { token });
        }

        // Google OAuth inicijalizacija
        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/api/auth/google-callback"
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Google OAuth callback
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded || result.Principal == null)
                return BadRequest("Google autentikacija nije uspjela.");

            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var name = result.Principal.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(email))
                return BadRequest("E-mail nije pronađen u Google profilu.");

            // Možeš ovdje dodati korisnika u bazu ako ne postoji

            var token = _jwt.GenerateToken("google_user_id", email);

            // Redirekcija na frontend (npr. dashboard) s JWT tokenom
            return Redirect($"{Request.Scheme}://{Request.Host}/dashboard?token={token}");
        }
    }

    public record LoginRequest(string Email, string Password);
}
