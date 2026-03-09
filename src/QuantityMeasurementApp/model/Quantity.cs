using System;

namespace QuantityMeasurementApp
{
    public class Quantity<TUnit> where TUnit : struct, Enum
    {
        private readonly double value;
        private readonly TUnit unit;

        public double Value => value;
        public TUnit Unit => unit;

        public Quantity(double value, TUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Value must be a finite number.", nameof(value));
            }

            this.value = value;
            this.unit = unit;
        }

        private static double ToBaseUnit(double numericValue, TUnit unit)
        {
            Type type = typeof(TUnit);

            if (type == typeof(LengthUnit))
            {
                LengthUnit lengthUnit = (LengthUnit)(object)unit;
                return lengthUnit.ConvertToBaseUnit(numericValue);
            }

            if (type == typeof(WeightUnit))
            {
                WeightUnit weightUnit = (WeightUnit)(object)unit;
                return weightUnit.ConvertToBaseUnit(numericValue);
            }

            if (type == typeof(VolumeUnit))
            {
                VolumeUnit volumeUnit = (VolumeUnit)(object)unit;
                return volumeUnit.ConvertToBaseUnit(numericValue);
            }

            throw new InvalidOperationException($"No base-unit conversion defined for unit type {type.Name}.");
        }

        private static double FromBaseUnit(double baseValue, TUnit unit)
        {
            Type type = typeof(TUnit);

            if (type == typeof(LengthUnit))
            {
                LengthUnit lengthUnit = (LengthUnit)(object)unit;
                return lengthUnit.ConvertFromBaseUnit(baseValue);
            }

            if (type == typeof(WeightUnit))
            {
                WeightUnit weightUnit = (WeightUnit)(object)unit;
                return weightUnit.ConvertFromBaseUnit(baseValue);
            }

            if (type == typeof(VolumeUnit))
            {
                VolumeUnit volumeUnit = (VolumeUnit)(object)unit;
                return volumeUnit.ConvertFromBaseUnit(baseValue);
            }

            throw new InvalidOperationException($"No base-unit conversion defined for unit type {type.Name}.");
        }

        public Quantity<TUnit> ConvertTo(TUnit targetUnit)
        {
            double baseValue = ToBaseUnit(value, unit);
            double convertedValue = FromBaseUnit(baseValue, targetUnit);

            return new Quantity<TUnit>(convertedValue, targetUnit);
        }

        public Quantity<TUnit> Add(Quantity<TUnit> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return Add(other, unit);
        }

        public Quantity<TUnit> Add(Quantity<TUnit> other, TUnit resultUnit)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            double firstBase = ToBaseUnit(value, unit);
            double secondBase = ToBaseUnit(other.value, other.unit);
            double sumBase = firstBase + secondBase;

            double resultValue = FromBaseUnit(sumBase, resultUnit);

            return new Quantity<TUnit>(resultValue, resultUnit);
        }

        // UC12: subtraction

        public Quantity<TUnit> Subtract(Quantity<TUnit> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return Subtract(other, unit);
        }

        public Quantity<TUnit> Subtract(Quantity<TUnit> other, TUnit resultUnit)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            double firstBase = ToBaseUnit(value, unit);
            double secondBase = ToBaseUnit(other.value, other.unit);
            double diffBase = firstBase - secondBase;

            double resultValue = FromBaseUnit(diffBase, resultUnit);

            return new Quantity<TUnit>(resultValue, resultUnit);
        }

        // UC12: division (dimensionless scalar)

        public double Divide(Quantity<TUnit> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            double firstBase = ToBaseUnit(value, unit);
            double secondBase = ToBaseUnit(other.value, other.unit);

            if (secondBase == 0.0)
            {
                throw new DivideByZeroException("Cannot divide by a quantity with zero base value.");
            }

            return firstBase / secondBase;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null || obj.GetType() != typeof(Quantity<TUnit>))
            {
                return false;
            }

            Quantity<TUnit> other = (Quantity<TUnit>)obj;

            double thisBase = ToBaseUnit(value, unit);
            double otherBase = ToBaseUnit(other.value, other.unit);

            return thisBase.CompareTo(otherBase) == 0;
        }

        public override int GetHashCode()
        {
            double baseValue = ToBaseUnit(value, unit);
            return baseValue.GetHashCode();
        }

        public override string ToString()
        {
            return $"{value} {unit}";
        }
    }
}