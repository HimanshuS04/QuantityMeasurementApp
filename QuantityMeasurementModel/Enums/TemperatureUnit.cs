using System;

namespace QuantityMeasurementApp
{
    public enum TemperatureUnit
    {
        Celsius,
        Fahrenheit,
        Kelvin
    }

    public static class TemperatureUnitExtensions
    {
        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            // Use Celsius as base unit for temperature
            switch (unit)
            {
                case TemperatureUnit.Celsius:
                    return value;
                case TemperatureUnit.Fahrenheit:
                    return (value - 32.0) * 5.0 / 9.0;
                case TemperatureUnit.Kelvin:
                    return value - 273.15;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unsupported temperature unit.");
            }
        }

        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValueInCelsius)
        {
            switch (unit)
            {
                case TemperatureUnit.Celsius:
                    return baseValueInCelsius;
                case TemperatureUnit.Fahrenheit:
                    return baseValueInCelsius * 9.0 / 5.0 + 32.0;
                case TemperatureUnit.Kelvin:
                    return baseValueInCelsius + 273.15;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unsupported temperature unit.");
            }
        }
    }
}