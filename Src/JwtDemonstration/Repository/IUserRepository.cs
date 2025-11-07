using JwtDemonstration.Models;

namespace JwtDemonstration.Repository;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<User> AddAsync(User user, CancellationToken ct = default);
    Task<bool> UsernameExistsAsync(string username, CancellationToken ct = default);
    IAsyncEnumerable<User> GetAllAsync(CancellationToken ct = default);
}
