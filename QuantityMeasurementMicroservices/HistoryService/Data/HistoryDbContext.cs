using Microsoft.EntityFrameworkCore;
using HistoryService.Models;

namespace HistoryService.Data
{
    public class HistoryDbContext : DbContext
    {
        public HistoryDbContext(DbContextOptions<HistoryDbContext> options) : base(options) { }

        public DbSet<OperationHistory> OperationHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var op = modelBuilder.Entity<OperationHistory>();
            op.ToTable("OperationHistories");
            op.HasKey(e => e.Id);
            op.Property(e => e.Id).ValueGeneratedOnAdd();
            op.Property(e => e.UserId).IsRequired(false);
            op.Property(e => e.Category).IsRequired();
            op.Property(e => e.OperationType).IsRequired().HasMaxLength(50);
            op.Property(e => e.FirstValue).IsRequired();
            op.Property(e => e.FirstUnit).IsRequired().HasMaxLength(50);
            op.Property(e => e.SecondValue).IsRequired(false);
            op.Property(e => e.SecondUnit).IsRequired(false).HasMaxLength(50);
            op.Property(e => e.ResultValue).IsRequired(false);
            op.Property(e => e.ResultUnit).IsRequired(false).HasMaxLength(50);
        }
    }
}