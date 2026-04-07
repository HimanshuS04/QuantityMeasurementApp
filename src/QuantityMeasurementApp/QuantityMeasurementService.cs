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
    }
}