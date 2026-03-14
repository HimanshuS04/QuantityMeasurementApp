// QuantityMeasurementController/Program.cs
namespace QuantityMeasurementApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // UC15: always use cache repository (now backed by JSON)
            IQuantityMeasurementRepository repository = QuantityMeasurementCacheRepository.Instance;

            // Single merged DTO-based service
            IQuantityMeasurementService service = new QuantityMeasurementService(repository);

            // Menu talks directly to IQuantityMeasurementService
            QuantityMenu quantityMenu = new QuantityMenu(service);

            quantityMenu.ShowMainMenu();
        }
    }
}