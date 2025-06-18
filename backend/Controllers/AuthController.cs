[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase {
    [HttpPost("login")] public IActionResult Login(LoginRequest req) { /* generate JWT */ }
    [HttpPost("google")] public IActionResult GoogleLogin(GoogleAuthRequest req) { /* Google OAuth */ }
}
