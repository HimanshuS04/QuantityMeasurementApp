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

        private static void ValidateValue(double numericValue, string parameterName)
        {
            if (double.IsNaN(numericValue) || double.IsInfinity(numericValue))
            {
                throw new ArgumentException("Value must be a finite number.", parameterName);
            }
        }

        private double ToBaseUnitInFeet()
        {
            return unit.ConvertToBaseUnit(value);
        }

        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            double convertedValue = Convert(value, unit, targetUnit);
            return new QuantityLength(convertedValue, targetUnit);
        }

        public static double Convert(double inputValue, LengthUnit sourceUnit, LengthUnit targetUnit)
        {
            ValidateValue(inputValue, nameof(inputValue));
            sourceUnit.ValidateUnit(nameof(sourceUnit));
            targetUnit.ValidateUnit(nameof(targetUnit));

            double baseValueInFeet = sourceUnit.ConvertToBaseUnit(inputValue);
            double convertedValue = targetUnit.ConvertFromBaseUnit(baseValueInFeet);

            return convertedValue;
        }

        public QuantityLength Add(QuantityLength other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return Add(this, other);
        }

        public static QuantityLength Add(QuantityLength first, QuantityLength second)
        {
            if (first is null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second is null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return Add(first, second, first.unit);
        }

        public static QuantityLength Add(QuantityLength first, QuantityLength second, LengthUnit resultUnit)
        {
            if (first is null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second is null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return Add(first.value, first.unit, second.value, second.unit, resultUnit);
        }

        public static QuantityLength Add(
            double firstValue,
            LengthUnit firstUnit,
            double secondValue,
            LengthUnit secondUnit,
            LengthUnit resultUnit)
        {
            ValidateValue(firstValue, nameof(firstValue));
            ValidateValue(secondValue, nameof(secondValue));
            firstUnit.ValidateUnit(nameof(firstUnit));
            secondUnit.ValidateUnit(nameof(secondUnit));
            resultUnit.ValidateUnit(nameof(resultUnit));

            double firstBase = firstUnit.ConvertToBaseUnit(firstValue);
            double secondBase = secondUnit.ConvertToBaseUnit(secondValue);
            double sumBase = firstBase + secondBase;

            double resultValue = resultUnit.ConvertFromBaseUnit(sumBase);

            return new QuantityLength(resultValue, resultUnit);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null || GetType() != obj.GetType())
            {
                return false;
            }

            QuantityLength other = (QuantityLength)obj;

            double thisBase = unit.ConvertToBaseUnit(value);
            double otherBase = other.unit.ConvertToBaseUnit(other.value);

            return thisBase.CompareTo(otherBase) == 0;
        }

        public override int GetHashCode()
        {
            return unit.ConvertToBaseUnit(value).GetHashCode();
        }
    }
}