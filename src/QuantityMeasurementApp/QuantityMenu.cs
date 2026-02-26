using System;

namespace QuantityMeasurementApp
{
    public class QuantityMenu
    {
        private readonly IQuantityMeasurementService quantityMeasurementService;

        public QuantityMenu(IQuantityMeasurementService quantityMeasurementService)
        {
            this.quantityMeasurementService = quantityMeasurementService
                                              ?? throw new ArgumentNullException(nameof(quantityMeasurementService));
        }

        public void ShowMainMenu()
        {
            bool shouldExit = false;

            while (!shouldExit)
            {
                Console.WriteLine("=== Quantity Measurement Application ===");
                Console.WriteLine("1. Compare two feet values");
                Console.WriteLine("2. Compare two inch values");
                Console.WriteLine("3. Compare two length quantities (generic equality)");
                Console.WriteLine("4. Convert between length units ");
                Console.WriteLine("5. Add two length quantities ");
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

                    case "3":
                        ExecuteGenericQuantityEqualityComparison();
                        break;

                    case "4":
                        ExecuteUnitConversion();
                        break;

                    case "5":
                        ExecuteUnitAddition();
                        break;

                    case "0":
                        shouldExit = true;
                        Console.WriteLine("Exiting application.");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please select 1, 2, 3, 4, 5 or 0.");
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
            Console.WriteLine($"Output: Equal ({areEqual.ToString().ToLowerInvariant()})");
        }

        private void ExecuteInchEqualityComparison()
        {
            double firstInchValue = ReadInchValue("Enter first value in inches: ");
            double secondInchValue = ReadInchValue("Enter second value in inches: ");

            bool areEqual = quantityMeasurementService.AreInchMeasurementsEqual(firstInchValue, secondInchValue);

            Console.WriteLine($"Input: {firstInchValue} inch and {secondInchValue} inch");
            Console.WriteLine($"Output: Equal ({areEqual.ToString().ToLowerInvariant()})");
        }

        private void ExecuteGenericQuantityEqualityComparison()
        {
            double firstValue = ReadNumericValue("Enter first value: ");
            LengthUnit firstUnit = ReadLengthUnit("Enter first unit (feet/inch/yard/cm): ");

            double secondValue = ReadNumericValue("Enter second value: ");
            LengthUnit secondUnit = ReadLengthUnit("Enter second unit (feet/inch/yard/cm): ");

            bool areEqual = quantityMeasurementService.AreQuantitiesEqual(firstValue, firstUnit, secondValue, secondUnit);

            Console.WriteLine($"Input: {firstValue} {firstUnit.ToString().ToLowerInvariant()} and {secondValue} {secondUnit.ToString().ToLowerInvariant()}");
            Console.WriteLine($"Output: Equal ({areEqual.ToString().ToLowerInvariant()})");
        }

        private void ExecuteUnitConversion()
        {
            double value = ReadNumericValue("Enter value: ");
            LengthUnit sourceUnit = ReadLengthUnit("Enter source unit (feet/inch/yard/cm): ");
            LengthUnit targetUnit = ReadLengthUnit("Enter target unit (feet/inch/yard/cm): ");

            double convertedValue = quantityMeasurementService.ConvertLength(value, sourceUnit, targetUnit);

            Console.WriteLine($"Converted: {value} {sourceUnit.ToString().ToLowerInvariant()} = {convertedValue} {targetUnit.ToString().ToLowerInvariant()}");
        }

        private void ExecuteUnitAddition()
        {
            double firstValue = ReadNumericValue("Enter first value: ");
            LengthUnit firstUnit = ReadLengthUnit("Enter first unit (feet/inch/yard/cm): ");

            double secondValue = ReadNumericValue("Enter second value: ");
            LengthUnit secondUnit = ReadLengthUnit("Enter second unit (feet/inch/yard/cm): ");

            LengthUnit resultUnit = ReadLengthUnit("Enter result unit (feet/inch/yard/cm): ");

            QuantityLength result = quantityMeasurementService.AddQuantities(
                firstValue,
                firstUnit,
                secondValue,
                secondUnit,
                resultUnit);

            Console.WriteLine(
                $"Addition result: {firstValue} {firstUnit.ToString().ToLowerInvariant()} + " +
                $"{secondValue} {secondUnit.ToString().ToLowerInvariant()} = " +
                $"{result.Value} {result.Unit.ToString().ToLowerInvariant()}");
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

        private double ReadNumericValue(string inputPrompt)
        {
            while (true)
            {
                Console.Write(inputPrompt);
                string? userInput = Console.ReadLine();

                if (double.TryParse(userInput, out double numericValue))
                {
                    return numericValue;
                }

                Console.WriteLine("Invalid input. Please enter a numeric value.");
            }
        }

        private LengthUnit ReadLengthUnit(string inputPrompt)
        {
            while (true)
            {
                Console.Write(inputPrompt);
                string? userInput = Console.ReadLine();

                if (string.Equals(userInput, "feet", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "foot", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "ft", StringComparison.OrdinalIgnoreCase))
                {
                    return LengthUnit.Feet;
                }

                if (string.Equals(userInput, "inch", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "inches", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "in", StringComparison.OrdinalIgnoreCase))
                {
                    return LengthUnit.Inch;
                }

                if (string.Equals(userInput, "yard", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "yards", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "yd", StringComparison.OrdinalIgnoreCase))
                {
                    return LengthUnit.Yard;
                }

                if (string.Equals(userInput, "centimeter", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "centimeters", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "cm", StringComparison.OrdinalIgnoreCase))
                {
                    return LengthUnit.Centimeter;
                }

                Console.WriteLine("Invalid unit. Please enter 'feet', 'inch', 'yard' or 'cm'.");
            }
        }
    }
}