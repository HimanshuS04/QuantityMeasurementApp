namespace QuantityMeasurementApp
{
    public interface IQuantityMeasurementService
    {
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

        bool AreWeightQuantitiesEqual(double firstValue, WeightUnit firstUnit, double secondValue, WeightUnit secondUnit);
        double ConvertWeight(double value, WeightUnit sourceUnit, WeightUnit targetUnit);
        QuantityWeight AddWeightQuantities(
            double firstValue,
            WeightUnit firstUnit,
            double secondValue,
            WeightUnit secondUnit,
            WeightUnit resultUnit);
    }
}