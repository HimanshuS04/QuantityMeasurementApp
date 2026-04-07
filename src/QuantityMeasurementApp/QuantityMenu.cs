using System;

namespace QuantityMeasurementApp
{
    public class QuantityMenu
    {
        private readonly IQuantityMeasurementService quantityMeasurementService;

        public QuantityMenu(IQuantityMeasurementService quantityMeasurementService)
        {
            this.quantityMeasurementService = quantityMeasurementService ?? throw new ArgumentNullException(nameof(quantityMeasurementService));
        }

        public void ShowMainMenu()
        {
            bool shouldExit = false;

            while (!shouldExit)
            {
                Console.WriteLine("=== Quantity Measurement Application ===");
                Console.WriteLine("1. Compare two feet values");
                Console.WriteLine("2. Compare two inch values");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string? userOption = Console.ReadLine();
                Console.WriteLine();

                switch (userOption)
                {
                    case "1":
                        ExecuteFeetEqualityComparison();
                        break;

                    case "2":
                        ExecuteInchEqualityComparison();
                        break;

                    case "0":
                        shouldExit = true;
                        Console.WriteLine("Exiting application.");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please select 1, 2 or 0.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void ExecuteFeetEqualityComparison()
        {
            double firstFeetValue = ReadFeetValue("Enter first value in feet: ");
            double secondFeetValue = ReadFeetValue("Enter second value in feet: ");

            bool areEqual = quantityMeasurementService.AreFeetMeasurementsEqual(firstFeetValue, secondFeetValue);

            Console.WriteLine($"Input: {firstFeetValue} ft and {secondFeetValue} ft");
            Console.WriteLine($"Output: {areEqual.ToString().ToLowerInvariant()}");
        }

        private void ExecuteInchEqualityComparison()
        {
            double firstInchValue = ReadInchValue("Enter first value in inches: ");
            double secondInchValue = ReadInchValue("Enter second value in inches: ");

            bool areEqual = quantityMeasurementService.AreInchMeasurementsEqual(firstInchValue, secondInchValue);

            Console.WriteLine($"Input: {firstInchValue} inch and {secondInchValue} inch");
            Console.WriteLine($"Output: {areEqual.ToString().ToLowerInvariant()}");
        }

        private double ReadFeetValue(string inputPrompt)
        {
            while (true)
            {
                Console.Write(inputPrompt);
                string? userInput = Console.ReadLine();

                if (double.TryParse(userInput, out double feetValue))
                {
                    return feetValue;
                }

                Console.WriteLine("Invalid input. Please enter a numeric value for feet.");
            }
        }

        private double ReadInchValue(string inputPrompt)
        {
            while (true)
            {
                Console.Write(inputPrompt);
                string? userInput = Console.ReadLine();

                if (double.TryParse(userInput, out double inchValue))
                {
                    return inchValue;
                }

                Console.WriteLine("Invalid input. Please enter a numeric value for inches.");
            }
        }
    }
}