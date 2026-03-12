using System;

namespace QuantityMeasurementApp
{
    public enum LengthUnit
    {
        Feet,
        Inch,
        Yard,
        Centimeter
    }

    public static class LengthUnitExtensions
    {
        private const double CentimetersToInchesFactor = 0.393701;

        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return value;
                case LengthUnit.Inch:
                    return value / 12.0;
                case LengthUnit.Yard:
                    return value * 3.0;
                case LengthUnit.Centimeter:
                    return value * (CentimetersToInchesFactor / 12.0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unsupported length unit.");
            }
        }

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValueInFeet)
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return baseValueInFeet;
                case LengthUnit.Inch:
                    return baseValueInFeet * 12.0;
                case LengthUnit.Yard:
                    return baseValueInFeet / 3.0;
                case LengthUnit.Centimeter:
                    return baseValueInFeet * (12.0 / CentimetersToInchesFactor);
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unsupported length unit.");
            }
        }

        public static void ValidateUnit(this LengthUnit unit, string parameterName)
        {
            if (!Enum.IsDefined(typeof(LengthUnit), unit))
            {
                throw new ArgumentOutOfRangeException(parameterName, unit, "Unsupported length unit.");
            }
        }
    }
}