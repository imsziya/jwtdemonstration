using JwtDemonstration.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtDemonstration.Repository;

public class UserRepository(ApplicationDbContext db) : IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default) =>
        await db.Users
            .SingleOrDefaultAsync(u => u.Username == username, ct);

    public async Task<User> AddAsync(User user, CancellationToken ct = default)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        return user;
    }

    public Task<bool> UsernameExistsAsync(string username, CancellationToken ct = default) =>
        db.Users.AnyAsync(u => u.Username == username, ct);

    public async IAsyncEnumerable<User> GetAllAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        await foreach (var u in db.Users.AsAsyncEnumerable().WithCancellation(ct))
            yield return u;
    }
}
