namespace QuantityService.Models.Enums
{
    public enum TemperatureUnit { Celsius, Fahrenheit, Kelvin }

    public static class TemperatureUnitExtensions
    {
        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value) => unit switch
        {
            TemperatureUnit.Celsius => value,
            TemperatureUnit.Fahrenheit => (value - 32.0) * 5.0 / 9.0,
            TemperatureUnit.Kelvin => value - 273.15,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseCelsius) => unit switch
        {
            TemperatureUnit.Celsius => baseCelsius,
            TemperatureUnit.Fahrenheit => baseCelsius * 9.0 / 5.0 + 32.0,
            TemperatureUnit.Kelvin => baseCelsius + 273.15,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };
    }
}