using System;

namespace QuantityMeasurementApp
{
    public enum WeightUnit
    {
        Kilogram,
        Gram,
        Pound
    }

    public static class WeightUnitExtensions
    {
        private const double PoundsToKilogramsFactor = 0.453592;

        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            switch (unit)
            {
                case WeightUnit.Kilogram:
                    return value;
                case WeightUnit.Gram:
                    return value * 0.001;
                case WeightUnit.Pound:
                    return value * PoundsToKilogramsFactor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unsupported weight unit.");
            }
        }

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValueInKilograms)
        {
            switch (unit)
            {
                case WeightUnit.Kilogram:
                    return baseValueInKilograms;
                case WeightUnit.Gram:
                    return baseValueInKilograms * 1000.0;
                case WeightUnit.Pound:
                    return baseValueInKilograms / PoundsToKilogramsFactor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unsupported weight unit.");
            }
        }

        public static void ValidateUnit(this WeightUnit unit, string parameterName)
        {
            if (!Enum.IsDefined(typeof(WeightUnit), unit))
            {
                throw new ArgumentOutOfRangeException(parameterName, unit, "Unsupported weight unit.");
            }
        }
    }
}