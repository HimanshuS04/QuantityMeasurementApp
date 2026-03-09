namespace QuantityMeasurementApp
{
    public interface IQuantityMeasurementService
    {
        // Length
        bool AreFeetMeasurementsEqual(double firstFeetValue, double secondFeetValue);
        bool AreInchMeasurementsEqual(double firstInchValue, double secondInchValue);
        bool AreQuantitiesEqual(double firstValue, LengthUnit firstUnit, double secondValue, LengthUnit secondUnit);
        double ConvertLength(double value, LengthUnit sourceUnit, LengthUnit targetUnit);
        QuantityLength AddQuantities(
            double firstValue,
            LengthUnit firstUnit,
            double secondValue,
            LengthUnit secondUnit,
            LengthUnit resultUnit);
        Quantity<LengthUnit> SubtractLength(
            double firstValue,
            LengthUnit firstUnit,
            double secondValue,
            LengthUnit secondUnit,
            LengthUnit resultUnit);
        double DivideLength(
            double firstValue,
            LengthUnit firstUnit,
            double secondValue,
            LengthUnit secondUnit);

        // Weight
        bool AreWeightQuantitiesEqual(double firstValue, WeightUnit firstUnit, double secondValue, WeightUnit secondUnit);
        double ConvertWeight(double value, WeightUnit sourceUnit, WeightUnit targetUnit);
        QuantityWeight AddWeightQuantities(
            double firstValue,
            WeightUnit firstUnit,
            double secondValue,
            WeightUnit secondUnit,
            WeightUnit resultUnit);
        Quantity<WeightUnit> SubtractWeight(
            double firstValue,
            WeightUnit firstUnit,
            double secondValue,
            WeightUnit secondUnit,
            WeightUnit resultUnit);
        double DivideWeight(
            double firstValue,
            WeightUnit firstUnit,
            double secondValue,
            WeightUnit secondUnit);

        // Volume
        bool AreVolumeQuantitiesEqual(double firstValue, VolumeUnit firstUnit, double secondValue, VolumeUnit secondUnit);
        double ConvertVolume(double value, VolumeUnit sourceUnit, VolumeUnit targetUnit);
        Quantity<VolumeUnit> AddVolumeQuantities(
            double firstValue,
            VolumeUnit firstUnit,
            double secondValue,
            VolumeUnit secondUnit,
            VolumeUnit resultUnit);
        Quantity<VolumeUnit> SubtractVolume(
            double firstValue,
            VolumeUnit firstUnit,
            double secondValue,
            VolumeUnit secondUnit,
            VolumeUnit resultUnit);
        double DivideVolume(
            double firstValue,
            VolumeUnit firstUnit,
            double secondValue,
            VolumeUnit secondUnit);
    }
}