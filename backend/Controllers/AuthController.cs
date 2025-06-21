using backend.Services;

public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login(User user)
    {
        // dummy autorizacija
        var token = _jwtService.GenerateToken(user.Id, user.Email);
        return Ok(new { token });
    }
}
