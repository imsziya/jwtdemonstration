using JwtDemonstration.Models;
using JwtDemonstration.Models.DTO;
using JwtDemonstration.Repository;
using System.Security.Cryptography;

namespace JwtDemonstration.Services;

public class UserService(IUserRepository repo) : IUserService
{
    public async Task<User> RegisterAsync(RegisterRequest req, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
            throw new ArgumentException("Username/password required.");

        if (await repo.UsernameExistsAsync(req.Username, ct))
            throw new InvalidOperationException("Username already exists.");

        CreatePasswordHash(req.Password, out var hash, out var salt);

        var user = new User
        {
            Username = req.Username,
            PasswordHash = hash,
            PasswordSalt = salt
        };
        // role default or provided
        var role = string.IsNullOrWhiteSpace(req.Role) ? "User" : req.Role!;
        user.Roles.Add(role);

        return await repo.AddAsync(user, ct);
    }

    public async Task<User?> ValidateLoginAsync(LoginRequest req, CancellationToken ct = default)
    {
        var user = await repo.GetByUsernameAsync(req.Username, ct);
        if (user is null) return null;
        return VerifyPassword(req.Password, user.PasswordHash, user.PasswordSalt) ? user : null;
    }

    public IAsyncEnumerable<User> GetAllAsync(CancellationToken ct = default) => repo.GetAllAsync(ct);

    // PBKDF2 helpers
    private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(16);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        hash = pbkdf2.GetBytes(32);
    }

    private static bool VerifyPassword(string password, byte[] storedHash, byte[] salt)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        var computed = pbkdf2.GetBytes(32);
        return CryptographicOperations.FixedTimeEquals(storedHash, computed);
    }
}
