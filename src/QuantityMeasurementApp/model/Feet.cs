namespace QuantityMeasurementApp
{
    public class Feet
    {
        private readonly QuantityLength quantityLength;

        public double Value => quantityLength.Value;

        public Feet(double value)
        {
            quantityLength = new QuantityLength(value, LengthUnit.Feet);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            Feet otherFeet = (Feet)obj;

            return quantityLength.Equals(otherFeet.quantityLength);
        }

        public override int GetHashCode()
        {
            return quantityLength.GetHashCode();
        }
    }
}