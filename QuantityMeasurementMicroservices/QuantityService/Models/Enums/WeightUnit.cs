namespace QuantityService.Models.Enums
{
    public enum WeightUnit { Kilogram, Gram, Pound }

    public static class WeightUnitExtensions
    {
        private const double LbToKg = 0.453592;

        public static double ConvertToBaseUnit(this WeightUnit unit, double value) => unit switch
        {
            WeightUnit.Kilogram => value,
            WeightUnit.Gram => value * 0.001,
            WeightUnit.Pound => value * LbToKg,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseKg) => unit switch
        {
            WeightUnit.Kilogram => baseKg,
            WeightUnit.Gram => baseKg * 1000.0,
            WeightUnit.Pound => baseKg / LbToKg,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };
    }
}