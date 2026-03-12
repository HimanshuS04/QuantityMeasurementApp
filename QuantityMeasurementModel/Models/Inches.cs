namespace QuantityMeasurementApp
{
    public class Inches
    {
        private readonly QuantityLength quantityLength;

        public double Value => quantityLength.Value;

        public Inches(double value)
        {
            quantityLength = new QuantityLength(value, LengthUnit.Inch);
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

            Inches otherInches = (Inches)obj;

            return quantityLength.Equals(otherInches.quantityLength);
        }

        public override int GetHashCode()
        {
            return quantityLength.GetHashCode();
        }
    }
}