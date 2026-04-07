namespace QuantityMeasurementApp
{
    public class Inches
    {
        private readonly double value;

        public double Value => value;

        public Inches(double value)
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

            Inches otherInches = (Inches)obj;
            return value.CompareTo(otherInches.value) == 0;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}