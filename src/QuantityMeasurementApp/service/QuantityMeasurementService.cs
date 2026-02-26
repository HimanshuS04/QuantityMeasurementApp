namespace QuantityMeasurementApp
{
    public class QuantityMeasurementService : IQuantityMeasurementService
    {
        public bool AreFeetMeasurementsEqual(double firstFeetValue, double secondFeetValue)
        {
            Feet firstFeet = new Feet(firstFeetValue);
            Feet secondFeet = new Feet(secondFeetValue);

            return firstFeet.Equals(secondFeet);
        }

        public bool AreInchMeasurementsEqual(double firstInchValue, double secondInchValue)
        {
            Inches firstInch = new Inches(firstInchValue);
            Inches secondInch = new Inches(secondInchValue);

            return firstInch.Equals(secondInch);
        }

        public bool AreQuantitiesEqual(double firstValue, LengthUnit firstUnit, double secondValue, LengthUnit secondUnit)
        {
            QuantityLength firstQuantity = new QuantityLength(firstValue, firstUnit);
            QuantityLength secondQuantity = new QuantityLength(secondValue, secondUnit);

            return firstQuantity.Equals(secondQuantity);
        }

        public double ConvertLength(double value, LengthUnit sourceUnit, LengthUnit targetUnit)
        {
            return QuantityLength.Convert(value, sourceUnit, targetUnit);
        }

        public QuantityLength AddQuantities(
            double firstValue,
            LengthUnit firstUnit,
            double secondValue,
            LengthUnit secondUnit,
            LengthUnit resultUnit)
        {
            return QuantityLength.Add(firstValue, firstUnit, secondValue, secondUnit, resultUnit);
        }

        public bool AreWeightQuantitiesEqual(double firstValue, WeightUnit firstUnit, double secondValue, WeightUnit secondUnit)
        {
            QuantityWeight firstWeight = new QuantityWeight(firstValue, firstUnit);
            QuantityWeight secondWeight = new QuantityWeight(secondValue, secondUnit);

            return firstWeight.Equals(secondWeight);
        }

        public double ConvertWeight(double value, WeightUnit sourceUnit, WeightUnit targetUnit)
        {
            return QuantityWeight.Convert(value, sourceUnit, targetUnit);
        }

        public QuantityWeight AddWeightQuantities(
            double firstValue,
            WeightUnit firstUnit,
            double secondValue,
            WeightUnit secondUnit,
            WeightUnit resultUnit)
        {
            return QuantityWeight.Add(firstValue, firstUnit, secondValue, secondUnit, resultUnit);
        }
    }
}