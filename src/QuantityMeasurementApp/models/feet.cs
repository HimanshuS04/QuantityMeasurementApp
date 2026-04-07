namespace QuantityMeasurementApp
{
    public class Feet
    {
        private readonly double value;

        public double Value => value;

        public Feet(double value)
        {
            this.value = value;
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
            return value.CompareTo(otherFeet.value) == 0;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}