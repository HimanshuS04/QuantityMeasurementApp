using Microsoft.EntityFrameworkCore;
using AuthService.Models;

namespace AuthService.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var user = modelBuilder.Entity<User>();
            user.ToTable("Users");
            user.HasKey(u => u.Id);
            user.Property(u => u.Email).IsRequired().HasMaxLength(256);
            user.HasIndex(u => u.Email).IsUnique();
            user.Property(u => u.PasswordHash).IsRequired();
            user.Property(u => u.PasswordSalt).IsRequired();
            user.Property(u => u.Role).IsRequired().HasMaxLength(50);
            user.Property(u => u.CreatedAt).IsRequired();
        }
    }
}