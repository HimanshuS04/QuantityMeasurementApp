using System;
using Microsoft.Extensions.Configuration;

namespace QuantityMeasurementApp
{
    public static class AppConfiguration
    {
        public static IQuantityMeasurementRepository CreateRepositoryFromConfig()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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

            Console.WriteLine("Using in-memory cache repository (no/invalid appsettings.json).");
            return QuantityMeasurementCacheRepository.Instance;
        }

        /// <summary>
        /// UC16: Creates a fully configured menu (via interface), including
        /// repository and service wiring. When ShowMainMenu returns, resources are released.
        /// </summary>
        public static IQuantityMenu CreateMenuFromConfig()
        {
            // 1) Determine repository from config
            IQuantityMeasurementRepository repository = CreateRepositoryFromConfig();

            // 2) Create the merged DTO-based service
            IQuantityMeasurementService service = new QuantityMeasurementService(repository);

            // 3) Create the concrete menu
            IQuantityMenu innerMenu = new QuantityMenu(service);

            // 4) Wrap it so that resources are released when the menu finishes
            return new ConfiguredMenu(innerMenu, repository);
        }

        /// <summary>
        /// Private wrapper that ensures ReleaseResources() is called after the menu finishes.
        /// </summary>
        private sealed class ConfiguredMenu : IQuantityMenu
        {
            private readonly IQuantityMenu innerMenu;
            private readonly IQuantityMeasurementRepository repository;

            public ConfiguredMenu(IQuantityMenu innerMenu, IQuantityMeasurementRepository repository)
            {
                this.innerMenu = innerMenu ?? throw new ArgumentNullException(nameof(innerMenu));
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public void ShowMainMenu()
            {
                try
                {
                    innerMenu.ShowMainMenu();
                }
                finally
                {
                    repository.ReleaseResources();
                }
            }
        }
    }
}