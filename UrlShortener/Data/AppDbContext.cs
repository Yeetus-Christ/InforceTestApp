using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ShortUrl> ShortUrls { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.ShortUrls)
                .WithOne(s => s.User);

            modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("string"),
                Role = "Admin"
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "user",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("string"),
                Role = "User"
            }
        );

        }
    }
}
