using System;
using Microsoft.Extensions.Configuration;

namespace QuantityMeasurementApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // UC16: repository selection moved to AppConfiguration
            IQuantityMeasurementRepository repository = AppConfiguration.CreateRepositoryFromConfig();
            
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