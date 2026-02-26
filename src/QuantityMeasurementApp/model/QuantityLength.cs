using System;

namespace QuantityMeasurementApp
{
    public class QuantityLength
    {
        private const double CentimetersToInchesFactor = 0.393701;

        private readonly double value;
        private readonly LengthUnit unit;

        public double Value => value;
        public LengthUnit Unit => unit;

        public QuantityLength(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        private static double GetFeetPerUnit(LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return 1.0;
                case LengthUnit.Inch:
                    return 1.0 / 12.0;
                case LengthUnit.Yard:
                    return 3.0;
                case LengthUnit.Centimeter:
                    return CentimetersToInchesFactor / 12.0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unsupported length unit.");
            }
        }

        private double ToBaseUnitInFeet()
        {
            double factor = GetFeetPerUnit(unit);
            return value * factor;
        }

        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            double convertedValue = Convert(value, unit, targetUnit);
            return new QuantityLength(convertedValue, targetUnit);
        }

        public static double Convert(double inputValue, LengthUnit sourceUnit, LengthUnit targetUnit)
        {
            if (double.IsNaN(inputValue) || double.IsInfinity(inputValue))
            {
                throw new ArgumentException("Value must be a finite number.", nameof(inputValue));
            }

            if (!Enum.IsDefined(typeof(LengthUnit), sourceUnit))
            {
                throw new ArgumentOutOfRangeException(nameof(sourceUnit), sourceUnit, "Unsupported length unit.");
            }

            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
            {
                throw new ArgumentOutOfRangeException(nameof(targetUnit), targetUnit, "Unsupported length unit.");
            }

            double sourceFactor = GetFeetPerUnit(sourceUnit);
            double targetFactor = GetFeetPerUnit(targetUnit);

            double baseValueInFeet = inputValue * sourceFactor;
            double convertedValue = baseValueInFeet / targetFactor;

            return convertedValue;
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