using System;

namespace QuantityMeasurementApp
{
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        public double Value => value;
        public LengthUnit Unit => unit;

        public QuantityLength(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        private double ToBaseUnitInFeet()
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return value;
                case LengthUnit.Inch:
                    return value / 12.0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unsupported length unit.");
            }
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

            QuantityLength otherQuantity = (QuantityLength)obj;

            double thisBaseValue = ToBaseUnitInFeet();
            double otherBaseValue = otherQuantity.ToBaseUnitInFeet();

            return thisBaseValue.CompareTo(otherBaseValue) == 0;
        }

        public override int GetHashCode()
        {
            return ToBaseUnitInFeet().GetHashCode();
        }
    }
}