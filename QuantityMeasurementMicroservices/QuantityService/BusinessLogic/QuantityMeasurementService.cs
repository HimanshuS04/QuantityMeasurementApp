using QuantityService.Exceptions;
using QuantityService.Models;
using QuantityService.Models.Enums;

namespace QuantityService.BusinessLogic
{
    public class QuantityMeasurementService : IQuantityMeasurementService
    {
        public bool CompareQuantities(QuantityDto first, QuantityDto second)
        {
            if (first.Category != second.Category)
                throw new QuantityMeasurementException("Cannot compare different categories.");

            double base1 = ToBase(first);
            double base2 = ToBase(second);
            return base1.CompareTo(base2) == 0;
        }

        public QuantityDto ConvertQuantity(QuantityDto quantity, string targetUnit)
        {
            double baseValue = ToBase(quantity);
            double converted = FromBase(quantity.Category, targetUnit, baseValue);
            return new QuantityDto { Category = quantity.Category, Unit = targetUnit, Value = converted };
        }

        public QuantityDto AddQuantities(QuantityDto first, QuantityDto second, string resultUnit)
        {
            ValidateSameCategory(first, second);
            ValidateNotTemperature(first.Category, "Addition");

            double sum = ToBase(first) + ToBase(second);
            double result = FromBase(first.Category, resultUnit, sum);
            return new QuantityDto { Category = first.Category, Unit = resultUnit, Value = result };
        }

        public QuantityDto SubtractQuantities(QuantityDto first, QuantityDto second, string resultUnit)
        {
            ValidateSameCategory(first, second);
            ValidateNotTemperature(first.Category, "Subtraction");

            double diff = ToBase(first) - ToBase(second);
            double result = FromBase(first.Category, resultUnit, diff);
            return new QuantityDto { Category = first.Category, Unit = resultUnit, Value = result };
        }

        public double DivideQuantities(QuantityDto first, QuantityDto second)
        {
            ValidateSameCategory(first, second);
            ValidateNotTemperature(first.Category, "Division");

            double base2 = ToBase(second);
            if (base2 == 0) throw new QuantityMeasurementException("Cannot divide by zero.");
            return ToBase(first) / base2;
        }

        private void ValidateSameCategory(QuantityDto first, QuantityDto second)
        {
            if (first.Category != second.Category)
                throw new QuantityMeasurementException("Categories must match.");
        }

        private void ValidateNotTemperature(MeasurementCategory category, string operation)
        {
            if (category == MeasurementCategory.Temperature)
                throw new QuantityMeasurementException($"{operation} not supported for temperature.");
        }

        private double ToBase(QuantityDto dto)
        {
            return dto.Category switch
            {
                MeasurementCategory.Length => ParseLength(dto.Unit).ConvertToBaseUnit(dto.Value),
                MeasurementCategory.Weight => ParseWeight(dto.Unit).ConvertToBaseUnit(dto.Value),
                MeasurementCategory.Volume => ParseVolume(dto.Unit).ConvertToBaseUnit(dto.Value),
                MeasurementCategory.Temperature => ParseTemperature(dto.Unit).ConvertToBaseUnit(dto.Value),
                _ => throw new QuantityMeasurementException("Unsupported category.")
            };
        }

        private double FromBase(MeasurementCategory category, string unit, double baseValue)
        {
            return category switch
            {
                MeasurementCategory.Length => ParseLength(unit).ConvertFromBaseUnit(baseValue),
                MeasurementCategory.Weight => ParseWeight(unit).ConvertFromBaseUnit(baseValue),
                MeasurementCategory.Volume => ParseVolume(unit).ConvertFromBaseUnit(baseValue),
                MeasurementCategory.Temperature => ParseTemperature(unit).ConvertFromBaseUnit(baseValue),
                _ => throw new QuantityMeasurementException("Unsupported category.")
            };
        }

        private static LengthUnit ParseLength(string unit) => unit.ToLower() switch
        {
            "feet" or "foot" or "ft" => LengthUnit.Feet,
            "inch" or "inches" or "in" => LengthUnit.Inch,
            "yard" or "yards" or "yd" => LengthUnit.Yard,
            "centimeter" or "centimeters" or "cm" => LengthUnit.Centimeter,
            _ => throw new QuantityMeasurementException($"Unsupported length unit: {unit}")
        };

        private static WeightUnit ParseWeight(string unit) => unit.ToLower() switch
        {
            "kilogram" or "kilograms" or "kg" => WeightUnit.Kilogram,
            "gram" or "grams" or "g" => WeightUnit.Gram,
            "pound" or "pounds" or "lb" or "lbs" => WeightUnit.Pound,
            _ => throw new QuantityMeasurementException($"Unsupported weight unit: {unit}")
        };

        private static VolumeUnit ParseVolume(string unit) => unit.ToLower() switch
        {
            "litre" or "liter" or "l" => VolumeUnit.Litre,
            "millilitre" or "milliliter" or "ml" => VolumeUnit.Millilitre,
            "gallon" or "gallons" or "gal" => VolumeUnit.Gallon,
            _ => throw new QuantityMeasurementException($"Unsupported volume unit: {unit}")
        };

        private static TemperatureUnit ParseTemperature(string unit) => unit.ToLower() switch
        {
            "celsius" or "c" => TemperatureUnit.Celsius,
            "fahrenheit" or "f" => TemperatureUnit.Fahrenheit,
            "kelvin" or "k" => TemperatureUnit.Kelvin,
            _ => throw new QuantityMeasurementException($"Unsupported temperature unit: {unit}")
        };
    }
}