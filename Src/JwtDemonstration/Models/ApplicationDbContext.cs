using Microsoft.EntityFrameworkCore;

namespace JwtDemonstration.Models;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Username).IsRequired();
            e.HasIndex(x => x.Username).IsUnique(); // enforce unique usernames
        });
    }
}