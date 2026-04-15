using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QuantityService.Data
{
    public class QuantityDbContextFactory : IDesignTimeDbContextFactory<QuantityDbContext>
    {
        public QuantityDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<QuantityDbContext>();
            string connString = configuration.GetConnectionString("QuantityDb")!;
            optionsBuilder.UseSqlServer(connString);

            return new QuantityDbContext(optionsBuilder.Options);
        }
    }
}