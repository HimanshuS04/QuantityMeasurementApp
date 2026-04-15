namespace QuantityService.Models.Enums
{
    public enum VolumeUnit { Litre, Millilitre, Gallon }

    public static class VolumeUnitExtensions
    {
        private const double GalToL = 3.78541;

        public static double ConvertToBaseUnit(this VolumeUnit unit, double value) => unit switch
        {
            VolumeUnit.Litre => value,
            VolumeUnit.Millilitre => value * 0.001,
            VolumeUnit.Gallon => value * GalToL,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseLitre) => unit switch
        {
            VolumeUnit.Litre => baseLitre,
            VolumeUnit.Millilitre => baseLitre * 1000.0,
            VolumeUnit.Gallon => baseLitre / GalToL,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };
    }
}