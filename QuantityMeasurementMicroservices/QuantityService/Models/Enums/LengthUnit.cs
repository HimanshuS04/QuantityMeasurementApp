namespace QuantityService.Models.Enums
{
    public enum LengthUnit { Feet, Inch, Yard, Centimeter }

    public static class LengthUnitExtensions
    {
        private const double CmToInFactor = 0.393701;

        public static double ConvertToBaseUnit(this LengthUnit unit, double value) => unit switch
        {
            LengthUnit.Feet => value,
            LengthUnit.Inch => value / 12.0,
            LengthUnit.Yard => value * 3.0,
            LengthUnit.Centimeter => value * (CmToInFactor / 12.0),
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseFeet) => unit switch
        {
            LengthUnit.Feet => baseFeet,
            LengthUnit.Inch => baseFeet * 12.0,
            LengthUnit.Yard => baseFeet / 3.0,
            LengthUnit.Centimeter => baseFeet * (12.0 / CmToInFactor),
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };
    }
}