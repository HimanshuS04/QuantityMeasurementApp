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

        private static void ValidateUnit(LengthUnit unit, string parameterName)
        {
            if (!Enum.IsDefined(typeof(LengthUnit), unit))
            {
                throw new ArgumentOutOfRangeException(parameterName, unit, "Unsupported length unit.");
            }
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
            ValidateValue(inputValue, nameof(inputValue));
            ValidateUnit(sourceUnit, nameof(sourceUnit));
            ValidateUnit(targetUnit, nameof(targetUnit));

            double sourceFactor = GetFeetPerUnit(sourceUnit);
            double targetFactor = GetFeetPerUnit(targetUnit);

            double baseValueInFeet = inputValue * sourceFactor;
            double convertedValue = baseValueInFeet / targetFactor;

            return convertedValue;
        }

        // UC6: instance addition in this.Unit
        public QuantityLength Add(QuantityLength other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return Add(this, other);
        }

        // UC6: static addition, result in first.Unit
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

        // UC7: static addition with explicit target unit (QuantityLength operands)
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

        // UC6/UC7: static addition using raw values and units, explicit resultUnit
        public static QuantityLength Add(
            double firstValue,
            LengthUnit firstUnit,
            double secondValue,
            LengthUnit secondUnit,
            LengthUnit resultUnit)
        {
            ValidateValue(firstValue, nameof(firstValue));
            ValidateValue(secondValue, nameof(secondValue));
            ValidateUnit(firstUnit, nameof(firstUnit));
            ValidateUnit(secondUnit, nameof(secondUnit));
            ValidateUnit(resultUnit, nameof(resultUnit));

            double firstBase = firstValue * GetFeetPerUnit(firstUnit);
            double secondBase = secondValue * GetFeetPerUnit(secondUnit);
            double sumBase = firstBase + secondBase;

            double resultFactor = GetFeetPerUnit(resultUnit);
            double resultValue = sumBase / resultFactor;

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

            double thisBase = ToBaseUnitInFeet();
            double otherBase = other.ToBaseUnitInFeet();

            return thisBase.CompareTo(otherBase) == 0;
        }

        public override int GetHashCode()
        {
            return ToBaseUnitInFeet().GetHashCode();
        }
    }
}