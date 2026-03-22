using System;

namespace QuantityMeasurementApp
{
    public class QuantityMenu: IQuantityMenu
    {
        private readonly IQuantityMeasurementService quantityMeasurementService;

        public QuantityMenu(IQuantityMeasurementService quantityMeasurementService)
        {
            this.quantityMeasurementService =
                quantityMeasurementService ??
                throw new ArgumentNullException(nameof(quantityMeasurementService));
        }
        // MAIN MENU

        public void ShowMainMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n=== Quantity Measurement Application ===");
                Console.WriteLine("1. Length");
                Console.WriteLine("2. Weight");
                Console.WriteLine("3. Volume");
                Console.WriteLine("4. Temperature");
                Console.WriteLine("0. Exit");

                Console.Write("Select measurement type: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ShowLengthMenu();
                        break;

                    case "2":
                        ShowWeightMenu();
                        break;

                    case "3":
                        ShowVolumeMenu();
                        break;

                    case "4":
                        ShowTemperatureMenu();
                        break;

                    case "0":
                        exit = true;
                        Console.WriteLine("Exiting application...");
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        // LENGTH MENU

        private void ShowLengthMenu()
        {
            bool back = false;

            while (!back)
            {
                Console.WriteLine("\n--- Length Operations ---");
                Console.WriteLine("1. Compare");
                Console.WriteLine("2. Convert");
                Console.WriteLine("3. Add");
                Console.WriteLine("4. Subtract");
                Console.WriteLine("5. Divide");
                Console.WriteLine("0. Back");

                Console.Write("Choose operation: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ExecuteGenericLengthEqualityComparison();
                        break;

                    case "2":
                        ExecuteLengthConversion();
                        break;

                    case "3":
                        ExecuteLengthAddition();
                        break;

                    case "4":
                        ExecuteLengthSubtraction();
                        break;

                    case "5":
                        ExecuteLengthDivision();
                        break;

                    case "0":
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        // WEIGHT MENU

        private void ShowWeightMenu()
        {
            bool back = false;

            while (!back)
            {
                Console.WriteLine("\n--- Weight Operations ---");
                Console.WriteLine("1. Compare");
                Console.WriteLine("2. Convert");
                Console.WriteLine("3. Add");
                Console.WriteLine("4. Subtract");
                Console.WriteLine("5. Divide");
                Console.WriteLine("0. Back");

                Console.Write("Choose operation: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ExecuteWeightEqualityComparison();
                        break;

                    case "2":
                        ExecuteWeightConversion();
                        break;

                    case "3":
                        ExecuteWeightAddition();
                        break;

                    case "4":
                        ExecuteWeightSubtraction();
                        break;

                    case "5":
                        ExecuteWeightDivision();
                        break;

                    case "0":
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        // VOLUME MENU

        private void ShowVolumeMenu()
        {
            bool back = false;

            while (!back)
            {
                Console.WriteLine("\n--- Volume Operations ---");
                Console.WriteLine("1. Compare");
                Console.WriteLine("2. Convert");
                Console.WriteLine("3. Add");
                Console.WriteLine("4. Subtract");
                Console.WriteLine("5. Divide");
                Console.WriteLine("0. Back");

                Console.Write("Choose operation: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ExecuteVolumeEqualityComparison();
                        break;

                    case "2":
                        ExecuteVolumeConversion();
                        break;

                    case "3":
                        ExecuteVolumeAddition();
                        break;

                    case "4":
                        ExecuteVolumeSubtraction();
                        break;

                    case "5":
                        ExecuteVolumeDivision();
                        break;

                    case "0":
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        // TEMPERATURE MENU

        private void ShowTemperatureMenu()
        {
            bool back = false;

            while (!back)
            {
                Console.WriteLine("\n--- Temperature Operations ---");
                Console.WriteLine("1. Compare");
                Console.WriteLine("2. Convert");
                Console.WriteLine("0. Back");

                Console.Write("Choose operation: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ExecuteTemperatureEqualityComparison();
                        break;

                    case "2":
                        ExecuteTemperatureConversion();
                        break;

                    case "0":
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }


        // Length
        private void ExecuteGenericLengthEqualityComparison()
        {
            double firstValue = ReadNumericValue("Enter first length value: ");
            LengthUnit firstUnitEnum = ReadLengthUnit("Enter first length unit(feet,inch,yard,cm): ");

            double secondValue = ReadNumericValue("Enter second length value: ");
            LengthUnit secondUnitEnum = ReadLengthUnit("Enter second length unit(feet,inch,yard,cm): ");

            string firstUnitName = LengthUnitToString(firstUnitEnum);
            string secondUnitName = LengthUnitToString(secondUnitEnum);

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Length,
                Unit = firstUnitName,
                Value = firstValue
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Length,
                Unit = secondUnitName,
                Value = secondValue
            };

            bool result = quantityMeasurementService.CompareQuantities(first, second);

            Console.WriteLine($"Equal ({result.ToString().ToLowerInvariant()})");
        }

        private void ExecuteLengthConversion()
        {
            double value = ReadNumericValue("Enter length value: ");

            LengthUnit sourceEnum = ReadLengthUnit("Enter source unit (feet,inch,yard,cm): ");
            LengthUnit targetEnum = ReadLengthUnit("Enter target unit (feet,inch,yard,cm): ");

            string source = LengthUnitToString(sourceEnum);
            string target = LengthUnitToString(targetEnum);

            QuantityDto input = new QuantityDto
            {
                Category = MeasurementCategory.Length,
                Unit = source,
                Value = value
            };

            QuantityDto result = quantityMeasurementService.ConvertQuantity(input, target);

            Console.WriteLine($"Converted: {value} {source} = {result.Value} {result.Unit}");
        }

        private void ExecuteLengthAddition()
        {
            double v1 = ReadNumericValue("Enter first value: ");
            LengthUnit u1 = ReadLengthUnit("Enter first unit(feet,inch,yard,cm): ");

            double v2 = ReadNumericValue("Enter second value: ");
            LengthUnit u2 = ReadLengthUnit("Enter second unit(feet,inch,yard,cm): ");

            LengthUnit resultEnum = ReadLengthUnit("Enter result unit: ");

            string unit1 = LengthUnitToString(u1);
            string unit2 = LengthUnitToString(u2);
            string resultUnit = LengthUnitToString(resultEnum);

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Length,
                Unit = unit1,
                Value = v1
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Length,
                Unit = unit2,
                Value = v2
            };

            QuantityDto result =
                quantityMeasurementService.AddQuantities(first, second, resultUnit);

            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }
        private void ExecuteLengthSubtraction()
        {
            double v1 = ReadNumericValue("Enter first value: ");
            LengthUnit u1 = ReadLengthUnit("Enter first unit (feet,inch,yard,cm): ");

            double v2 = ReadNumericValue("Enter second value: ");
            LengthUnit u2 = ReadLengthUnit("Enter second unit (feet,inch,yard,cm): ");

            LengthUnit resultEnum = ReadLengthUnit("Enter result unit: ");

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Length,
                Unit = LengthUnitToString(u1),
                Value = v1
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Length,
                Unit = LengthUnitToString(u2),
                Value = v2
            };

            QuantityDto result =
                quantityMeasurementService.SubtractQuantities(first, second, LengthUnitToString(resultEnum));

            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }
        private void ExecuteLengthDivision()
        {
            double v1 = ReadNumericValue("Enter first value: ");
            LengthUnit u1 = ReadLengthUnit("Enter first unit (feet,inch,yard,cm): ");

            double v2 = ReadNumericValue("Enter second value: ");
            LengthUnit u2 = ReadLengthUnit("Enter second unit (feet,inch,yard,cm): ");

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Length,
                Unit = LengthUnitToString(u1),
                Value = v1
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Length,
                Unit = LengthUnitToString(u2),
                Value = v2
            };

            double ratio = quantityMeasurementService.DivideQuantities(first, second);

            Console.WriteLine($"Result: {ratio}");
        }

    // =========================
        // WEIGHT OPERATIONS
        // =========================

        private void ExecuteWeightEqualityComparison()
        {
            double v1 = ReadNumericValue("Enter first weight value: ");
            WeightUnit u1 = ReadWeightUnit("Enter first weight unit: (kg/g/lb)");

            double v2 = ReadNumericValue("Enter second weight value: ");
            WeightUnit u2 = ReadWeightUnit("Enter second weight unit: (kg/g/lb) ");

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Weight,
                Unit = WeightUnitToString(u1),
                Value = v1
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Weight,
                Unit = WeightUnitToString(u2),
                Value = v2
            };

            bool result = quantityMeasurementService.CompareQuantities(first, second);

            Console.WriteLine($"Equal ({result.ToString().ToLowerInvariant()})");
        }
        private void ExecuteWeightConversion()
        {
            double value = ReadNumericValue("Enter weight value: ");
            WeightUnit sourceUnit = ReadWeightUnit("Enter source weight unit (kg/g/lb): ");
            WeightUnit targetUnit = ReadWeightUnit("Enter target weight unit (kg/g/lb): ");

            QuantityDto input = new QuantityDto
            {
                Category = MeasurementCategory.Weight,
                Unit = WeightUnitToString(sourceUnit),
                Value = value
            };

            QuantityDto result =
                quantityMeasurementService.ConvertQuantity(input, WeightUnitToString(targetUnit));

            Console.WriteLine($"Converted: {value} {input.Unit} = {result.Value} {result.Unit}");
        }

        private void ExecuteWeightAddition()
        {
            double firstValue = ReadNumericValue("Enter first weight value: ");
            WeightUnit firstUnit = ReadWeightUnit("Enter first weight unit (kg/g/lb): ");

            double secondValue = ReadNumericValue("Enter second weight value: ");
            WeightUnit secondUnit = ReadWeightUnit("Enter second weight unit (kg/g/lb): ");

            WeightUnit resultUnit = ReadWeightUnit("Enter result weight unit (kg/g/lb): ");

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Weight,
                Unit = WeightUnitToString(firstUnit),
                Value = firstValue
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Weight,
                Unit = WeightUnitToString(secondUnit),
                Value = secondValue
            };

            QuantityDto result =
                quantityMeasurementService.AddQuantities(first, second, WeightUnitToString(resultUnit));

            Console.WriteLine(
                $"Weight addition: {firstValue} {first.Unit} + {secondValue} {second.Unit} = {result.Value} {result.Unit}");
        }

        private void ExecuteWeightSubtraction()
        {
            double firstValue = ReadNumericValue("Enter first weight value: ");
            WeightUnit firstUnit = ReadWeightUnit("Enter first weight unit (kg/g/lb): ");

            double secondValue = ReadNumericValue("Enter second weight value: ");
            WeightUnit secondUnit = ReadWeightUnit("Enter second weight unit (kg/g/lb): ");

            WeightUnit resultUnit = ReadWeightUnit("Enter result weight unit (kg/g/lb): ");

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Weight,
                Unit = WeightUnitToString(firstUnit),
                Value = firstValue
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Weight,
                Unit = WeightUnitToString(secondUnit),
                Value = secondValue
            };

            QuantityDto result =
                quantityMeasurementService.SubtractQuantities(first, second, WeightUnitToString(resultUnit));

            Console.WriteLine(
                $"Weight subtraction: {firstValue} {first.Unit} - {secondValue} {second.Unit} = {result.Value} {result.Unit}");
        }

        private void ExecuteWeightDivision()
        {
            double firstValue = ReadNumericValue("Enter first weight value: ");
            WeightUnit firstUnit = ReadWeightUnit("Enter first weight unit (kg/g/lb): ");

            double secondValue = ReadNumericValue("Enter second weight value: ");
            WeightUnit secondUnit = ReadWeightUnit("Enter second weight unit (kg/g/lb): ");

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Weight,
                Unit = WeightUnitToString(firstUnit),
                Value = firstValue
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Weight,
                Unit = WeightUnitToString(secondUnit),
                Value = secondValue
            };

            double ratio =
                quantityMeasurementService.DivideQuantities(first, second);

            Console.WriteLine(
                $"Weight division: {firstValue} {first.Unit} / {secondValue} {second.Unit} = {ratio}");
        }

        // =========================
        // VOLUME OPERATIONS
        // =========================

        private void ExecuteVolumeEqualityComparison()
        {
            double firstValue = ReadNumericValue("Enter first volume value: ");
            VolumeUnit firstUnit = ReadVolumeUnit("Enter first volume unit (litre/ml/gal): ");

            double secondValue = ReadNumericValue("Enter second volume value: ");
            VolumeUnit secondUnit = ReadVolumeUnit("Enter second volume unit (litre/ml/gal): ");

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Volume,
                Unit = VolumeUnitToString(firstUnit),
                Value = firstValue
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Volume,
                Unit = VolumeUnitToString(secondUnit),
                Value = secondValue
            };

            bool result = quantityMeasurementService.CompareQuantities(first, second);

            Console.WriteLine($"Input: {firstValue} {first.Unit} and {secondValue} {second.Unit}");
            Console.WriteLine($"Output: Equal ({result.ToString().ToLowerInvariant()})");
        }

        private void ExecuteVolumeConversion()
        {
            double value = ReadNumericValue("Enter volume value: ");

            VolumeUnit sourceUnit = ReadVolumeUnit("Enter source unit (litre/ml/gal): ");
            VolumeUnit targetUnit = ReadVolumeUnit("Enter target unit(litre/ml/gal): ");

            QuantityDto input = new QuantityDto
            {
                Category = MeasurementCategory.Volume,
                Unit = VolumeUnitToString(sourceUnit),
                Value = value
            };

            QuantityDto result =
                quantityMeasurementService.ConvertQuantity(input, VolumeUnitToString(targetUnit));

            Console.WriteLine($"Converted: {result.Value} {result.Unit}");
        }

        private void ExecuteVolumeAddition()
        {
            double firstValue = ReadNumericValue("Enter first volume value: ");
            VolumeUnit firstUnit = ReadVolumeUnit("Enter first volume unit (litre/ml/gal): ");

            double secondValue = ReadNumericValue("Enter second volume value: ");
            VolumeUnit secondUnit = ReadVolumeUnit("Enter second volume unit (litre/ml/gal): ");

            VolumeUnit resultUnit = ReadVolumeUnit("Enter result volume unit (litre/ml/gal): ");

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Volume,
                Unit = VolumeUnitToString(firstUnit),
                Value = firstValue
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Volume,
                Unit = VolumeUnitToString(secondUnit),
                Value = secondValue
            };

            QuantityDto result =
                quantityMeasurementService.AddQuantities(first, second, VolumeUnitToString(resultUnit));

            Console.WriteLine(
                $"Volume addition: {firstValue} {first.Unit} + {secondValue} {second.Unit} = {result.Value} {result.Unit}");
        }

        private void ExecuteVolumeSubtraction()
        {
            double firstValue = ReadNumericValue("Enter first volume value: ");
            VolumeUnit firstUnit = ReadVolumeUnit("Enter first volume unit (litre/ml/gal): ");

            double secondValue = ReadNumericValue("Enter second volume value: ");
            VolumeUnit secondUnit = ReadVolumeUnit("Enter second volume unit (litre/ml/gal): ");

            VolumeUnit resultUnit = ReadVolumeUnit("Enter result volume unit (litre/ml/gal): ");

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Volume,
                Unit = VolumeUnitToString(firstUnit),
                Value = firstValue
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Volume,
                Unit = VolumeUnitToString(secondUnit),
                Value = secondValue
            };

            QuantityDto result =
                quantityMeasurementService.SubtractQuantities(first, second, VolumeUnitToString(resultUnit));

            Console.WriteLine(
                $"Volume subtraction: {firstValue} {first.Unit} - {secondValue} {second.Unit} = {result.Value} {result.Unit}");
        }

        private void ExecuteVolumeDivision()
        {
            double firstValue = ReadNumericValue("Enter first volume value: ");
            VolumeUnit firstUnit = ReadVolumeUnit("Enter first volume unit (litre/ml/gal): ");

            double secondValue = ReadNumericValue("Enter second volume value: ");
            VolumeUnit secondUnit = ReadVolumeUnit("Enter second volume unit (litre/ml/gal): ");

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Volume,
                Unit = VolumeUnitToString(firstUnit),
                Value = firstValue
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Volume,
                Unit = VolumeUnitToString(secondUnit),
                Value = secondValue
            };

            double ratio =
                quantityMeasurementService.DivideQuantities(first, second);

            Console.WriteLine(
                $"Volume division: {firstValue} {first.Unit} / {secondValue} {second.Unit} = {ratio}");
        }

        // =========================
        // TEMPERATURE
        // =========================

        private void ExecuteTemperatureEqualityComparison()
        {
            double firstValue = ReadNumericValue("Enter first temperature value: ");
            TemperatureUnit firstUnit = ReadTemperatureUnit("Enter first temperature unit (celsius,fahrenheit,kelvin): ");

            double secondValue = ReadNumericValue("Enter second temperature value: ");
            TemperatureUnit secondUnit = ReadTemperatureUnit("Enter second temperature unit (celsius,fahrenheit,kelvin): ");

            QuantityDto first = new QuantityDto
            {
                Category = MeasurementCategory.Temperature,
                Unit = TemperatureUnitToString(firstUnit),
                Value = firstValue
            };

            QuantityDto second = new QuantityDto
            {
                Category = MeasurementCategory.Temperature,
                Unit = TemperatureUnitToString(secondUnit),
                Value = secondValue
            };

            bool result =
                quantityMeasurementService.CompareQuantities(first, second);

            Console.WriteLine($"Equal ({result.ToString().ToLowerInvariant()})");
        }

        private void ExecuteTemperatureConversion()
        {
            double value = ReadNumericValue("Enter temperature value: ");

            TemperatureUnit sourceUnit = ReadTemperatureUnit("Enter source temperature unit (celsius,fahrenheit,kelvin): ");
            TemperatureUnit targetUnit = ReadTemperatureUnit("Enter target temperature unit (celsius,fahrenheit,kelvin): ");

            QuantityDto input = new QuantityDto
            {
                Category = MeasurementCategory.Temperature,
                Unit = TemperatureUnitToString(sourceUnit),
                Value = value
            };

            QuantityDto result =
                quantityMeasurementService.ConvertQuantity(input, TemperatureUnitToString(targetUnit));

            Console.WriteLine($"Converted: {value} {input.Unit} = {result.Value} {result.Unit}");
        }

        // =========================
        // UNIT STRING MAPPERS
        // =========================
        private static string LengthUnitToString(LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => "feet",
                LengthUnit.Inch => "inch",
                LengthUnit.Yard => "yard",
                LengthUnit.Centimeter => "cm",
                _ => throw new Exception("Invalid length unit")
            };
        }
        private static string WeightUnitToString(WeightUnit unit) =>
            unit switch
            {
                WeightUnit.Kilogram => "kg",
                WeightUnit.Gram => "g",
                WeightUnit.Pound => "lb",
                _ => throw new Exception("Invalid weight unit")
            };

        private static string VolumeUnitToString(VolumeUnit unit) =>
            unit switch
            {
                VolumeUnit.Litre => "litre",
                VolumeUnit.Millilitre => "ml",
                VolumeUnit.Gallon => "gallon",
                _ => throw new Exception("Invalid volume unit")
            };

        private static string TemperatureUnitToString(TemperatureUnit unit) =>
            unit switch
            {
                TemperatureUnit.Celsius => "celsius",
                TemperatureUnit.Fahrenheit => "fahrenheit",
                TemperatureUnit.Kelvin => "kelvin",
                _ => throw new Exception("Invalid temperature unit")
            };
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