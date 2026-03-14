using System;
using Microsoft.Extensions.Configuration;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// UC16: Application-level configuration helper.
    /// Reads appsettings.json and creates the appropriate repository.
    /// </summary>
    public static class AppConfiguration
    {
        public static IQuantityMeasurementRepository CreateRepositoryFromConfig()
        {
            // Load configuration from appsettings.json
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string repositoryType = configuration["Repository:Type"] ?? "Cache";

            if (string.Equals(repositoryType, "Database", StringComparison.OrdinalIgnoreCase))
            {
                string? connectionString = configuration.GetConnectionString("QuantityMeasurementDb");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    Console.WriteLine("Connection string 'QuantityMeasurementDb' is missing in appsettings.json. Falling back to in-memory cache repository.");
                    return QuantityMeasurementCacheRepository.Instance;
                }

                int maxPoolSize = 20;
                string? maxPoolSizeValue = configuration["Database:MaxPoolSize"];
                if (int.TryParse(maxPoolSizeValue, out int parsed))
                {
                    maxPoolSize = parsed;
                }

                Console.WriteLine("Using SQL Server database repository.");
                return new QuantityMeasurementDatabaseRepository(connectionString, maxPoolSize);
            }

            Console.WriteLine("Using in-memory cache repository.");
            return QuantityMeasurementCacheRepository.Instance;
        }
    }
}