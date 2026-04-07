using Microsoft.EntityFrameworkCore;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementDbContext : DbContext
    {
        public QuantityMeasurementDbContext(DbContextOptions<QuantityMeasurementDbContext> options)
            : base(options)
        {
        }

        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; } = null!;
        public DbSet<QuantityOperation> QuantityOperations { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Logs
            var log = modelBuilder.Entity<QuantityMeasurementEntity>();
            log.ToTable("QuantityMeasurementLogs");
            log.HasKey(e => e.Id);
            log.Property(e => e.Timestamp).HasColumnName("TimestampUtc").IsRequired();
            log.Property(e => e.OperationType).IsRequired().HasMaxLength(100);
            log.Property(e => e.Details).IsRequired();
            log.Property(e => e.HasError).IsRequired();
            log.Property(e => e.ErrorMessage).IsRequired(false);
            log.Property(e => e.Category).IsRequired(false);

            // Operations
            var op = modelBuilder.Entity<QuantityOperation>();
            op.ToTable("QuantityOperations");
            op.HasKey(e => e.Id);
            op.Property(e => e.Id).ValueGeneratedOnAdd();
            op.Property(e => e.Category).IsRequired();
            op.Property(e => e.OperationType).IsRequired().HasMaxLength(50);
            op.Property(e => e.FirstValue).IsRequired();
            op.Property(e => e.FirstUnit).IsRequired().HasMaxLength(50);
            op.Property(e => e.SecondValue).IsRequired(false);
            op.Property(e => e.SecondUnit).IsRequired(false).HasMaxLength(50);
            op.Property(e => e.ResultValue).IsRequired(false);
            op.Property(e => e.ResultUnit).IsRequired(false).HasMaxLength(50);

            // Map UserId (added in step 2)
            op.Property(e => e.UserId).IsRequired(false);

            // Users (UC18)
            var user = modelBuilder.Entity<User>();
            user.ToTable("Users");
            user.HasKey(u => u.Id);

            user.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            user.HasIndex(u => u.Email)
                .IsUnique();

            user.Property(u => u.PasswordHash)
                .IsRequired();

            user.Property(u => u.PasswordSalt)
                .IsRequired();

            user.Property(u => u.CreatedAt)
                .IsRequired();

            user.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}