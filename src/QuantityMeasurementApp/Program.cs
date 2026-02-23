namespace QuantityMeasurementApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IQuantityMeasurementService quantityMeasurementService = new QuantityMeasurementService();
            QuantityMenu quantityMenu = new QuantityMenu(quantityMeasurementService);

            quantityMenu.ShowMainMenu();
        }
    }
}