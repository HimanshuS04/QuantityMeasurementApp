using System;

namespace QuantityMeasurementApp
{
    public class QuantityWeight
    {
        private readonly double value;
        private readonly WeightUnit unit;

        public double Value => value;
        public WeightUnit Unit => unit;

        public QuantityWeight(double value, WeightUnit unit)
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

        private double ToBaseUnitInKilograms()
        {
            return unit.ConvertToBaseUnit(value);
        }

        public QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            double convertedValue = Convert(value, unit, targetUnit);
            return new QuantityWeight(convertedValue, targetUnit);
        }

        public static double Convert(double inputValue, WeightUnit sourceUnit, WeightUnit targetUnit)
        {
            ValidateValue(inputValue, nameof(inputValue));
            sourceUnit.ValidateUnit(nameof(sourceUnit));
            targetUnit.ValidateUnit(nameof(targetUnit));

            double baseValue = sourceUnit.ConvertToBaseUnit(inputValue);
            double convertedValue = targetUnit.ConvertFromBaseUnit(baseValue);

            return convertedValue;
        }

        public QuantityWeight Add(QuantityWeight other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return Add(this, other);
        }

        public static QuantityWeight Add(QuantityWeight first, QuantityWeight second)
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

        public static QuantityWeight Add(QuantityWeight first, QuantityWeight second, WeightUnit resultUnit)
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

        public static QuantityWeight Add(
            double firstValue,
            WeightUnit firstUnit,
            double secondValue,
            WeightUnit secondUnit,
            WeightUnit resultUnit)
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

            return new QuantityWeight(resultValue, resultUnit);
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

            QuantityWeight other = (QuantityWeight)obj;

            double thisBase = unit.ConvertToBaseUnit(value);
            double otherBase = other.unit.ConvertToBaseUnit(other.value);

            return thisBase.CompareTo(otherBase) == 0;
        }

        public override int GetHashCode()
        {
            return unit.ConvertToBaseUnit(value).GetHashCode();
        }

        public override string ToString()
        {
            return $"{value} {unit}";
        }
    }
}