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
                Console.WriteLine("4. Convert between length units");
                Console.WriteLine("5. Add two length quantities");
                Console.WriteLine("6. Compare two weight quantities");
                Console.WriteLine("7. Convert between weight units");
                Console.WriteLine("8. Add two weight quantities");
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
                        ExecuteGenericLengthEqualityComparison();
                        break;
                    case "4":
                        ExecuteLengthConversion();
                        break;
                    case "5":
                        ExecuteLengthAddition();
                        break;
                    case "6":
                        ExecuteWeightEqualityComparison();
                        break;
                    case "7":
                        ExecuteWeightConversion();
                        break;
                    case "8":
                        ExecuteWeightAddition();
                        break;
                    case "0":
                        shouldExit = true;
                        Console.WriteLine("Exiting application.");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please select 1–8 or 0.");
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

        private void ExecuteGenericLengthEqualityComparison()
        {
            double firstValue = ReadNumericValue("Enter first length value: ");
            LengthUnit firstUnit = ReadLengthUnit("Enter first length unit (feet/inch/yard/cm): ");

            double secondValue = ReadNumericValue("Enter second length value: ");
            LengthUnit secondUnit = ReadLengthUnit("Enter second length unit (feet/inch/yard/cm): ");

            bool areEqual = quantityMeasurementService.AreQuantitiesEqual(firstValue, firstUnit, secondValue, secondUnit);

            Console.WriteLine($"Input: {firstValue} {firstUnit.ToString().ToLowerInvariant()} and {secondValue} {secondUnit.ToString().ToLowerInvariant()}");
            Console.WriteLine($"Output: Equal ({areEqual.ToString().ToLowerInvariant()})");
        }

        private void ExecuteLengthConversion()
        {
            double value = ReadNumericValue("Enter length value: ");
            LengthUnit sourceUnit = ReadLengthUnit("Enter source length unit (feet/inch/yard/cm): ");
            LengthUnit targetUnit = ReadLengthUnit("Enter target length unit (feet/inch/yard/cm): ");

            double convertedValue = quantityMeasurementService.ConvertLength(value, sourceUnit, targetUnit);

            Console.WriteLine($"Converted: {value} {sourceUnit.ToString().ToLowerInvariant()} = {convertedValue} {targetUnit.ToString().ToLowerInvariant()}");
        }

        private void ExecuteLengthAddition()
        {
            double firstValue = ReadNumericValue("Enter first length value: ");
            LengthUnit firstUnit = ReadLengthUnit("Enter first length unit (feet/inch/yard/cm): ");

            double secondValue = ReadNumericValue("Enter second length value: ");
            LengthUnit secondUnit = ReadLengthUnit("Enter second length unit (feet/inch/yard/cm): ");

            LengthUnit resultUnit = ReadLengthUnit("Enter result length unit (feet/inch/yard/cm): ");

            QuantityLength result = quantityMeasurementService.AddQuantities(
                firstValue,
                firstUnit,
                secondValue,
                secondUnit,
                resultUnit);

            Console.WriteLine(
                $"Length addition: {firstValue} {firstUnit.ToString().ToLowerInvariant()} + " +
                $"{secondValue} {secondUnit.ToString().ToLowerInvariant()} = " +
                $"{result.Value} {result.Unit.ToString().ToLowerInvariant()}");
        }

        private void ExecuteWeightEqualityComparison()
        {
            double firstValue = ReadNumericValue("Enter first weight value: ");
            WeightUnit firstUnit = ReadWeightUnit("Enter first weight unit (kg/g/lb): ");

            double secondValue = ReadNumericValue("Enter second weight value: ");
            WeightUnit secondUnit = ReadWeightUnit("Enter second weight unit (kg/g/lb): ");

            bool areEqual = quantityMeasurementService.AreWeightQuantitiesEqual(firstValue, firstUnit, secondValue, secondUnit);

            Console.WriteLine($"Input: {firstValue} {firstUnit.ToString().ToLowerInvariant()} and {secondValue} {secondUnit.ToString().ToLowerInvariant()}");
            Console.WriteLine($"Output: Equal ({areEqual.ToString().ToLowerInvariant()})");
        }

        private void ExecuteWeightConversion()
        {
            double value = ReadNumericValue("Enter weight value: ");
            WeightUnit sourceUnit = ReadWeightUnit("Enter source weight unit (kg/g/lb): ");
            WeightUnit targetUnit = ReadWeightUnit("Enter target weight unit (kg/g/lb): ");

            double convertedValue = quantityMeasurementService.ConvertWeight(value, sourceUnit, targetUnit);

            Console.WriteLine($"Converted: {value} {sourceUnit.ToString().ToLowerInvariant()} = {convertedValue} {targetUnit.ToString().ToLowerInvariant()}");
        }

        private void ExecuteWeightAddition()
        {
            double firstValue = ReadNumericValue("Enter first weight value: ");
            WeightUnit firstUnit = ReadWeightUnit("Enter first weight unit (kg/g/lb): ");

            double secondValue = ReadNumericValue("Enter second weight value: ");
            WeightUnit secondUnit = ReadWeightUnit("Enter second weight unit (kg/g/lb): ");

            WeightUnit resultUnit = ReadWeightUnit("Enter result weight unit (kg/g/lb): ");

            QuantityWeight result = quantityMeasurementService.AddWeightQuantities(
                firstValue,
                firstUnit,
                secondValue,
                secondUnit,
                resultUnit);

            Console.WriteLine(
                $"Weight addition: {firstValue} {firstUnit.ToString().ToLowerInvariant()} + " +
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

                Console.WriteLine("Invalid length unit. Please enter 'feet', 'inch', 'yard' or 'cm'.");
            }
        }

        private WeightUnit ReadWeightUnit(string inputPrompt)
        {
            while (true)
            {
                Console.Write(inputPrompt);
                string? userInput = Console.ReadLine();

                if (string.Equals(userInput, "kilogram", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "kilograms", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "kg", StringComparison.OrdinalIgnoreCase))
                {
                    return WeightUnit.Kilogram;
                }

                if (string.Equals(userInput, "gram", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "grams", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "g", StringComparison.OrdinalIgnoreCase))
                {
                    return WeightUnit.Gram;
                }

                if (string.Equals(userInput, "pound", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "pounds", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "lb", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "lbs", StringComparison.OrdinalIgnoreCase))
                {
                    return WeightUnit.Pound;
                }

                Console.WriteLine("Invalid weight unit. Please enter 'kg', 'g' or 'lb'.");
            }
        }
    }
}