using JwtDemonstration.Models.DTO;
using JwtDemonstration.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtDemonstration.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService users) : ControllerBase
{
    [HttpGet("me", Name = "GetMe")]
    [Authorize]
    public IActionResult Me()
    {
        var name = User.Identity?.Name ?? "unknown";
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value);
        return Ok(new { id, username = name, roles });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll(CancellationToken ct)
    {
        var list = new List<UserDto>();
        await foreach (var u in users.GetAllAsync(ct))
            list.Add(new UserDto(u.Id, u.Username, u.Roles));

        return Ok(list);
    }
}
