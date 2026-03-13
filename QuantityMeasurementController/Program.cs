using System;
using Microsoft.Extensions.Configuration;

namespace QuantityMeasurementApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Load configuration from appsettings.json
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string repositoryType = configuration["Repository:Type"] ?? "Cache";

            IQuantityMeasurementRepository repository;

            if (string.Equals(repositoryType, "Database", StringComparison.OrdinalIgnoreCase))
            {
                string? connectionString = configuration.GetConnectionString("QuantityMeasurementDb");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    Console.WriteLine("Connection string 'QuantityMeasurementDb' is missing in appsettings.json. Falling back to in-memory cache repository.");
                    repository = QuantityMeasurementCacheRepository.Instance;
                }
                else
                {
                    int maxPoolSize = 20;
                    string? maxPoolSizeValue = configuration["Database:MaxPoolSize"];
                    if (int.TryParse(maxPoolSizeValue, out int parsed))
                    {
                        maxPoolSize = parsed;
                    }

                    repository = new QuantityMeasurementDatabaseRepository(connectionString, maxPoolSize);
                    Console.WriteLine("Using SQL Server database repository.");
                }
            }
            else
            {
                repository = QuantityMeasurementCacheRepository.Instance;
                Console.WriteLine("Using in-memory cache repository.");
            }

            // Domain service (UC14)
            IQuantityMeasurementService domainService = new QuantityMeasurementService(repository);

            // Controller
            QuantityMenu quantityMenu = new QuantityMenu(domainService);

            quantityMenu.ShowMainMenu();

            // Clean up resources if needed (DB connections, etc.)
            repository.ReleaseResources();
        }
    }
}