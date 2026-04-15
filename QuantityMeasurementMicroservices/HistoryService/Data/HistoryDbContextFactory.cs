using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HistoryService.Data
{
    public class HistoryDbContextFactory : IDesignTimeDbContextFactory<HistoryDbContext>
    {
        public HistoryDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HistoryDbContext>();
            string connString = configuration.GetConnectionString("HistoryDb")!;
            optionsBuilder.UseSqlServer(connString);

            return new HistoryDbContext(optionsBuilder.Options);
        }
    }
}