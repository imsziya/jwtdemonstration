using JwtDemonstration.Models;
using JwtDemonstration.Models.DTO;

namespace JwtDemonstration.Services;

public interface IUserService
{
    Task<User> RegisterAsync(RegisterRequest req, CancellationToken ct = default);
    Task<User?> ValidateLoginAsync(LoginRequest req, CancellationToken ct = default);
    IAsyncEnumerable<User> GetAllAsync(CancellationToken ct = default);
}
