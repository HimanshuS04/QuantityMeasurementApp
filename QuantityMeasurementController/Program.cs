namespace QuantityMeasurementApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Repository layer
            IQuantityMeasurementRepository repository = QuantityMeasurementCacheRepository.Instance;

            // Domain service (UC14)
            IQuantityMeasurementService domainService = new QuantityMeasurementService(repository);

            // DTO-based UC15 service
            IQuantityMeasurementDtoService dtoService = new QuantityMeasurementDtoService(domainService);

            // Controller
            QuantityMenu quantityMenu = new QuantityMenu(dtoService);

            quantityMenu.ShowMainMenu();
        }
    }
}