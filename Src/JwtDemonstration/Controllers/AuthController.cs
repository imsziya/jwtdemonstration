using JwtDemonstration.Models.DTO;
using JwtDemonstration.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtDemonstration.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _users;
    private readonly ITokenService _tokens;

    public AuthController(IUserService users, ITokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req, CancellationToken ct)
    {
        try
        {
            var user = await _users.RegisterAsync(req, ct);
            var dto = new UserDto(user.Id, user.Username, user.Roles);
            return CreatedAtRoute("GetMe", null, dto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        var user = await _users.ValidateLoginAsync(req, ct);
        if (user is null) return Unauthorized(new { message = "Invalid credentials." });

        var tokenResponse = _tokens.CreateAccessToken(user);
        return Ok(new AuthResponse(tokenResponse.AccessToken, "Bearer", tokenResponse.ExpiresOn));
    }
}
