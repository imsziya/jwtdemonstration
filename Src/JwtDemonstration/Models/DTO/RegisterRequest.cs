namespace JwtDemonstration.Models.DTO;

public record RegisterRequest(string Username, string Password, string? Role = null);
public record LoginRequest(string Username, string Password);
public record UserDto(Guid Id, string Username, IEnumerable<string> Roles);
public record AuthResponse(string AccessToken, string TokenType, double ExpiresIn);