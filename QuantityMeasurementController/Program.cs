namespace QuantityMeasurementApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IQuantityMeasurementRepository repository = QuantityMeasurementCacheRepository.Instance;
            IQuantityMeasurementService service = new QuantityMeasurementService(repository);
            QuantityMenu quantityMenu = new QuantityMenu(service);
            quantityMenu.ShowMainMenu();
        }
    }
}