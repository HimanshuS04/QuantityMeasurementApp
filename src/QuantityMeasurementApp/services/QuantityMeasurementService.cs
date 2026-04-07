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
    }
}