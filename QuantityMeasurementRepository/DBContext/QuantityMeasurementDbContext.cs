using Microsoft.EntityFrameworkCore;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// EF Core DbContext for the Web API path and Redis-backed repository.
    /// Holds:
    /// - QuantityMeasurementLogs (log/history entries)
    /// - QuantityOperations (structured operations)
    /// - Users (local application users)
    /// </summary>
    public class QuantityMeasurementDbContext : DbContext
    {
        public QuantityMeasurementDbContext(DbContextOptions<QuantityMeasurementDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Log/history entries (used by Redis-backed repository and any log queries).
        /// </summary>
        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; } = null!;

        /// <summary>
        /// Structured operations table: QuantityOperations.
        /// </summary>
        public DbSet<QuantityOperation> QuantityOperations { get; set; } = null!;

        /// <summary>
        /// Local users table: Users.
        /// </summary>
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === Log entity mapping (QuantityMeasurementLogs) ===
            var log = modelBuilder.Entity<QuantityMeasurementEntity>();

            // Map to log table (physical table: QuantityMeasurementLogs)
            log.ToTable("QuantityMeasurementLogs");

            log.HasKey(e => e.Id);

            log.Property(e => e.Timestamp)
               .HasColumnName("TimestampUtc")
               .IsRequired();

            log.Property(e => e.OperationType)
               .IsRequired()
               .HasMaxLength(100);

            log.Property(e => e.Details)
               .IsRequired();

            log.Property(e => e.HasError)
               .IsRequired();

            log.Property(e => e.ErrorMessage)
               .IsRequired(false);

            log.Property(e => e.Category)
               .IsRequired(false);

            // === Operations entity mapping (QuantityOperations) ===
            var op = modelBuilder.Entity<QuantityOperation>();

            // Map to main operations table and explicitly disable SQL OUTPUT clause
            // because the table has triggers (to avoid SQL error 334).
            op.ToTable("QuantityOperations", tb => tb.UseSqlOutputClause(false));

            op.HasKey(e => e.Id);

            // Id is INT IDENTITY in SQL
            op.Property(e => e.Id)
              .ValueGeneratedOnAdd();

            op.Property(e => e.Category)
              .IsRequired();

            op.Property(e => e.OperationType)
              .IsRequired()
              .HasMaxLength(50);

            op.Property(e => e.FirstValue)
              .IsRequired();

            op.Property(e => e.FirstUnit)
              .IsRequired()
              .HasMaxLength(50);

            op.Property(e => e.SecondValue)
              .IsRequired(false);

            op.Property(e => e.SecondUnit)
              .IsRequired(false)
              .HasMaxLength(50);

            op.Property(e => e.ResultValue)
              .IsRequired(false);

            op.Property(e => e.ResultUnit)
              .IsRequired(false)
              .HasMaxLength(50);

            // === User entity mapping (Users) ===
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
        }
    }
}