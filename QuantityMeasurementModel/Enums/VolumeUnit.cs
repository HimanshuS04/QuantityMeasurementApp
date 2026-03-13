using System;

namespace QuantityMeasurementApp
{
    public enum VolumeUnit
    {
        Litre,
        Millilitre,
        Gallon
    }

    public static class VolumeUnitExtensions
    {
        private const double GallonsToLitresFactor = 3.78541;

        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            switch (unit)
            {
                case VolumeUnit.Litre:
                    return value;
                case VolumeUnit.Millilitre:
                    return value * 0.001;
                case VolumeUnit.Gallon:
                    return value * GallonsToLitresFactor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unsupported volume unit.");
            }
        }

        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValueInLitres)
        {
            switch (unit)
            {
                case VolumeUnit.Litre:
                    return baseValueInLitres;
                case VolumeUnit.Millilitre:
                    return baseValueInLitres * 1000.0;
                case VolumeUnit.Gallon:
                    return baseValueInLitres / GallonsToLitresFactor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unsupported volume unit.");
            }
        }

        public static void ValidateUnit(this VolumeUnit unit, string parameterName)
        {
            if (!Enum.IsDefined(typeof(VolumeUnit), unit))
            {
                throw new ArgumentOutOfRangeException(parameterName, unit, "Unsupported volume unit.");
            }
        }
    }
}