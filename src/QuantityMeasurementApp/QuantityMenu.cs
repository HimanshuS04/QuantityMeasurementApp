using System;

namespace QuantityMeasurementApp
{
    public class QuantityMenu
    {
        private readonly IQuantityMeasurementService quantityMeasurementService;

        public QuantityMenu(IQuantityMeasurementService quantityMeasurementService)
        {
            this.quantityMeasurementService = quantityMeasurementService  ?? throw new ArgumentNullException(nameof(quantityMeasurementService));
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
                Console.WriteLine("9. Compare two volume quantities");
                Console.WriteLine("10. Convert between volume units");
                Console.WriteLine("11. Add two volume quantities");
                Console.WriteLine("12. Subtract two length quantities");
                Console.WriteLine("13. Divide two length quantities");
                Console.WriteLine("14. Subtract two weight quantities");
                Console.WriteLine("15. Divide two weight quantities");
                Console.WriteLine("16. Subtract two volume quantities");
                Console.WriteLine("17. Divide two volume quantities");
                Console.WriteLine("18. Compare two temperature quantities");
                Console.WriteLine("19. Convert between temperature units");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string userOption = Console.ReadLine();
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
                    case "9":
                        ExecuteVolumeEqualityComparison();
                        break;
                    case "10":
                        ExecuteVolumeConversion();
                        break;
                    case "11":
                        ExecuteVolumeAddition();
                        break;
                    case "12":
                        ExecuteLengthSubtraction();
                        break;
                    case "13":
                        ExecuteLengthDivision();
                        break;
                    case "14":
                        ExecuteWeightSubtraction();
                        break;
                    case "15":
                        ExecuteWeightDivision();
                        break;
                    case "16":
                        ExecuteVolumeSubtraction();
                        break;
                    case "17":
                        ExecuteVolumeDivision();
                        break;
                    case "18":
                        ExecuteTemperatureEqualityComparison();
                        break;
                    case "19":
                        ExecuteTemperatureConversion();
                        break;
                    case "0":
                        shouldExit = true;
                        Console.WriteLine("Exiting application.");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please select 1–11 or 0.");
                        break;
                }

                Console.WriteLine();
            }
        }

        // Length

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

        // Weight

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

        // Volume (UC11)

        private void ExecuteVolumeEqualityComparison()
        {
            double firstValue = ReadNumericValue("Enter first volume value: ");
            VolumeUnit firstUnit = ReadVolumeUnit("Enter first volume unit (litre/ml/gal): ");

            double secondValue = ReadNumericValue("Enter second volume value: ");
            VolumeUnit secondUnit = ReadVolumeUnit("Enter second volume unit (litre/ml/gal): ");

            bool areEqual = quantityMeasurementService.AreVolumeQuantitiesEqual(firstValue, firstUnit, secondValue, secondUnit);

            Console.WriteLine($"Input: {firstValue} {firstUnit.ToString().ToLowerInvariant()} and {secondValue} {secondUnit.ToString().ToLowerInvariant()}");
            Console.WriteLine($"Output: Equal ({areEqual.ToString().ToLowerInvariant()})");
        }

        private void ExecuteVolumeConversion()
        {
            double value = ReadNumericValue("Enter volume value: ");
            VolumeUnit sourceUnit = ReadVolumeUnit("Enter source volume unit (litre/ml/gal): ");
            VolumeUnit targetUnit = ReadVolumeUnit("Enter target volume unit (litre/ml/gal): ");

            double convertedValue = quantityMeasurementService.ConvertVolume(value, sourceUnit, targetUnit);

            Console.WriteLine($"Converted: {value} {sourceUnit.ToString().ToLowerInvariant()} = {convertedValue} {targetUnit.ToString().ToLowerInvariant()}");
        }

        private void ExecuteVolumeAddition()
        {
            double firstValue = ReadNumericValue("Enter first volume value: ");
            VolumeUnit firstUnit = ReadVolumeUnit("Enter first volume unit (litre/ml/gal): ");

            double secondValue = ReadNumericValue("Enter second volume value: ");
            VolumeUnit secondUnit = ReadVolumeUnit("Enter second volume unit (litre/ml/gal): ");

            VolumeUnit resultUnit = ReadVolumeUnit("Enter result volume unit (litre/ml/gal): ");

            Quantity<VolumeUnit> result = quantityMeasurementService.AddVolumeQuantities(
                firstValue,
                firstUnit,
                secondValue,
                secondUnit,
                resultUnit);

            Console.WriteLine(
                $"Volume addition: {firstValue} {firstUnit.ToString().ToLowerInvariant()} + " +
                $"{secondValue} {secondUnit.ToString().ToLowerInvariant()} = " +
                $"{result.Value} {result.Unit.ToString().ToLowerInvariant()}");
        }

        // Common readers

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

        private VolumeUnit ReadVolumeUnit(string inputPrompt)
        {
            while (true)
            {
                Console.Write(inputPrompt);
                string? userInput = Console.ReadLine();

                if (string.Equals(userInput, "litre", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "liter", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "litres", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "liters", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "l", StringComparison.OrdinalIgnoreCase))
                {
                    return VolumeUnit.Litre;
                }

                if (string.Equals(userInput, "millilitre", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "milliliter", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "millilitres", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "milliliters", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "ml", StringComparison.OrdinalIgnoreCase))
                {
                    return VolumeUnit.Millilitre;
                }

                if (string.Equals(userInput, "gallon", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "gallons", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(userInput, "gal", StringComparison.OrdinalIgnoreCase))
                {
                    return VolumeUnit.Gallon;
                }

                Console.WriteLine("Invalid volume unit. Please enter 'litre', 'ml' or 'gallon'.");
            }
        }
        // Length subtraction/division

        private void ExecuteLengthSubtraction()
        {
            double firstValue = ReadNumericValue("Enter first length value: ");
            LengthUnit firstUnit = ReadLengthUnit("Enter first length unit (feet/inch/yard/cm): ");

            double secondValue = ReadNumericValue("Enter second length value: ");
            LengthUnit secondUnit = ReadLengthUnit("Enter second length unit (feet/inch/yard/cm): ");

            LengthUnit resultUnit = ReadLengthUnit("Enter result length unit (feet/inch/yard/cm): ");

            Quantity<LengthUnit> result = quantityMeasurementService.SubtractLength(
                firstValue, firstUnit, secondValue, secondUnit, resultUnit);

            Console.WriteLine(
                $"Length subtraction: {firstValue} {firstUnit.ToString().ToLowerInvariant()} - " +
                $"{secondValue} {secondUnit.ToString().ToLowerInvariant()} = " +
                $"{result.Value} {result.Unit.ToString().ToLowerInvariant()}");
        }

        private void ExecuteLengthDivision()
        {
            double firstValue = ReadNumericValue("Enter first length value: ");
            LengthUnit firstUnit = ReadLengthUnit("Enter first length unit (feet/inch/yard/cm): ");

            double secondValue = ReadNumericValue("Enter second length value: ");
            LengthUnit secondUnit = ReadLengthUnit("Enter second length unit (feet/inch/yard/cm): ");

            double ratio = quantityMeasurementService.DivideLength(firstValue, firstUnit, secondValue, secondUnit);

            Console.WriteLine(
                $"Length division: {firstValue} {firstUnit.ToString().ToLowerInvariant()} / " +
                $"{secondValue} {secondUnit.ToString().ToLowerInvariant()} = {ratio}");
        }

        // Weight subtraction/division

        private void ExecuteWeightSubtraction()
        {
            double firstValue = ReadNumericValue("Enter first weight value: ");
            WeightUnit firstUnit = ReadWeightUnit("Enter first weight unit (kg/g/lb): ");

            double secondValue = ReadNumericValue("Enter second weight value: ");
            WeightUnit secondUnit = ReadWeightUnit("Enter second weight unit (kg/g/lb): ");

            WeightUnit resultUnit = ReadWeightUnit("Enter result weight unit (kg/g/lb): ");

            Quantity<WeightUnit> result = quantityMeasurementService.SubtractWeight(
                firstValue, firstUnit, secondValue, secondUnit, resultUnit);

            Console.WriteLine(
                $"Weight subtraction: {firstValue} {firstUnit.ToString().ToLowerInvariant()} - " +
                $"{secondValue} {secondUnit.ToString().ToLowerInvariant()} = " +
                $"{result.Value} {result.Unit.ToString().ToLowerInvariant()}");
        }

        private void ExecuteWeightDivision()
        {
            double firstValue = ReadNumericValue("Enter first weight value: ");
            WeightUnit firstUnit = ReadWeightUnit("Enter first weight unit (kg/g/lb): ");

            double secondValue = ReadNumericValue("Enter second weight value: ");
            WeightUnit secondUnit = ReadWeightUnit("Enter second weight unit (kg/g/lb): ");

            double ratio = quantityMeasurementService.DivideWeight(firstValue, firstUnit, secondValue, secondUnit);

            Console.WriteLine(
                $"Weight division: {firstValue} {firstUnit.ToString().ToLowerInvariant()} / " +
                $"{secondValue} {secondUnit.ToString().ToLowerInvariant()} = {ratio}");
        }

        // Volume subtraction/division

        private void ExecuteVolumeSubtraction()
        {
            double firstValue = ReadNumericValue("Enter first volume value: ");
            VolumeUnit firstUnit = ReadVolumeUnit("Enter first volume unit (litre/ml/gal): ");

            double secondValue = ReadNumericValue("Enter second volume value: ");
            VolumeUnit secondUnit = ReadVolumeUnit("Enter second volume unit (litre/ml/gal): ");

            VolumeUnit resultUnit = ReadVolumeUnit("Enter result volume unit (litre/ml/gal): ");

            Quantity<VolumeUnit> result = quantityMeasurementService.SubtractVolume(
                firstValue, firstUnit, secondValue, secondUnit, resultUnit);

            Console.WriteLine(
                $"Volume subtraction: {firstValue} {firstUnit.ToString().ToLowerInvariant()} - " +
                $"{secondValue} {secondUnit.ToString().ToLowerInvariant()} = " +
                $"{result.Value} {result.Unit.ToString().ToLowerInvariant()}");
        }

        private void ExecuteVolumeDivision()
        {
            double firstValue = ReadNumericValue("Enter first volume value: ");
            VolumeUnit firstUnit = ReadVolumeUnit("Enter first volume unit (litre/ml/gal): ");

            double secondValue = ReadNumericValue("Enter second volume value: ");
            VolumeUnit secondUnit = ReadVolumeUnit("Enter second volume unit (litre/ml/gal): ");

            double ratio = quantityMeasurementService.DivideVolume(firstValue, firstUnit, secondValue, secondUnit);

            Console.WriteLine(
                $"Volume division: {firstValue} {firstUnit.ToString().ToLowerInvariant()} / " +
                $"{secondValue} {secondUnit.ToString().ToLowerInvariant()} = {ratio}");
        }
        private void ExecuteTemperatureEqualityComparison()
{
    double firstValue = ReadNumericValue("Enter first temperature value: ");
    TemperatureUnit firstUnit = ReadTemperatureUnit("Enter first temperature unit (celsius/fahrenheit/kelvin): ");

    double secondValue = ReadNumericValue("Enter second temperature value: ");
    TemperatureUnit secondUnit = ReadTemperatureUnit("Enter second temperature unit (celsius/fahrenheit/kelvin): ");

    bool areEqual = quantityMeasurementService.AreTemperatureQuantitiesEqual(firstValue, firstUnit, secondValue, secondUnit);

    Console.WriteLine($"Input: {firstValue} {firstUnit.ToString().ToLowerInvariant()} and {secondValue} {secondUnit.ToString().ToLowerInvariant()}");
    Console.WriteLine($"Output: Equal ({areEqual.ToString().ToLowerInvariant()})");
}

private void ExecuteTemperatureConversion()
{
    double value = ReadNumericValue("Enter temperature value: ");
    TemperatureUnit sourceUnit = ReadTemperatureUnit("Enter source temperature unit (celsius/fahrenheit/kelvin): ");
    TemperatureUnit targetUnit = ReadTemperatureUnit("Enter target temperature unit (celsius/fahrenheit/kelvin): ");

    double convertedValue = quantityMeasurementService.ConvertTemperature(value, sourceUnit, targetUnit);

    Console.WriteLine($"Converted: {value} {sourceUnit.ToString().ToLowerInvariant()} = {convertedValue} {targetUnit.ToString().ToLowerInvariant()}");
}

private TemperatureUnit ReadTemperatureUnit(string inputPrompt)
{
    while (true)
    {
        Console.Write(inputPrompt);
        string? userInput = Console.ReadLine();

        if (string.Equals(userInput, "celsius", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(userInput, "c", StringComparison.OrdinalIgnoreCase))
        {
            return TemperatureUnit.Celsius;
        }

        if (string.Equals(userInput, "fahrenheit", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(userInput, "f", StringComparison.OrdinalIgnoreCase))
        {
            return TemperatureUnit.Fahrenheit;
        }

        if (string.Equals(userInput, "kelvin", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(userInput, "k", StringComparison.OrdinalIgnoreCase))
        {
            return TemperatureUnit.Kelvin;
        }

        Console.WriteLine("Invalid temperature unit. Please enter 'celsius', 'fahrenheit', or 'kelvin'.");
    }
}
    }
}