using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// UC15 DTO-based service that adapts QuantityDto calls to the UC14 domain service.
    /// </summary>
    public class QuantityMeasurementDtoService : IQuantityMeasurementDtoService
    {
        private readonly IQuantityMeasurementService domainService;

        public QuantityMeasurementDtoService(IQuantityMeasurementService domainService)
        {
            this.domainService = domainService ?? throw new ArgumentNullException(nameof(domainService));
        }

        public bool CompareQuantities(QuantityDto firstQuantity, QuantityDto secondQuantity)
        {
            if (firstQuantity == null) throw new ArgumentNullException(nameof(firstQuantity));
            if (secondQuantity == null) throw new ArgumentNullException(nameof(secondQuantity));

            if (firstQuantity.Category != secondQuantity.Category)
            {
                throw new QuantityMeasurementException("Cannot compare quantities of different categories.");
            }

            switch (firstQuantity.Category)
            {
                case MeasurementCategory.Length:
                {
                    LengthUnit u1 = ParseLengthUnit(firstQuantity.Unit);
                    LengthUnit u2 = ParseLengthUnit(secondQuantity.Unit);
                    return domainService.AreQuantitiesEqual(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                case MeasurementCategory.Weight:
                {
                    WeightUnit u1 = ParseWeightUnit(firstQuantity.Unit);
                    WeightUnit u2 = ParseWeightUnit(secondQuantity.Unit);
                    return domainService.AreWeightQuantitiesEqual(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                case MeasurementCategory.Volume:
                {
                    VolumeUnit u1 = ParseVolumeUnit(firstQuantity.Unit);
                    VolumeUnit u2 = ParseVolumeUnit(secondQuantity.Unit);
                    return domainService.AreVolumeQuantitiesEqual(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                case MeasurementCategory.Temperature:
                {
                    TemperatureUnit u1 = ParseTemperatureUnit(firstQuantity.Unit);
                    TemperatureUnit u2 = ParseTemperatureUnit(secondQuantity.Unit);
                    return domainService.AreTemperatureQuantitiesEqual(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                default:
                    throw new QuantityMeasurementException("Unsupported measurement category.");
            }
        }

        public QuantityDto ConvertQuantity(QuantityDto quantity, string targetUnit)
        {
            if (quantity == null) throw new ArgumentNullException(nameof(quantity));
            if (string.IsNullOrWhiteSpace(targetUnit)) throw new ArgumentException("Target unit is required.", nameof(targetUnit));

            switch (quantity.Category)
            {
                case MeasurementCategory.Length:
                {
                    LengthUnit source = ParseLengthUnit(quantity.Unit);
                    LengthUnit target = ParseLengthUnit(targetUnit);
                    double resultValue = domainService.ConvertLength(quantity.Value, source, target);

                    return new QuantityDto
                    {
                        Category = MeasurementCategory.Length,
                        Unit = targetUnit,
                        Value = resultValue
                    };
                }

                case MeasurementCategory.Weight:
                {
                    WeightUnit source = ParseWeightUnit(quantity.Unit);
                    WeightUnit target = ParseWeightUnit(targetUnit);
                    double resultValue = domainService.ConvertWeight(quantity.Value, source, target);

                    return new QuantityDto
                    {
                        Category = MeasurementCategory.Weight,
                        Unit = targetUnit,
                        Value = resultValue
                    };
                }

                case MeasurementCategory.Volume:
                {
                    VolumeUnit source = ParseVolumeUnit(quantity.Unit);
                    VolumeUnit target = ParseVolumeUnit(targetUnit);
                    double resultValue = domainService.ConvertVolume(quantity.Value, source, target);

                    return new QuantityDto
                    {
                        Category = MeasurementCategory.Volume,
                        Unit = targetUnit,
                        Value = resultValue
                    };
                }

                case MeasurementCategory.Temperature:
                {
                    TemperatureUnit source = ParseTemperatureUnit(quantity.Unit);
                    TemperatureUnit target = ParseTemperatureUnit(targetUnit);
                    double resultValue = domainService.ConvertTemperature(quantity.Value, source, target);

                    return new QuantityDto
                    {
                        Category = MeasurementCategory.Temperature,
                        Unit = targetUnit,
                        Value = resultValue
                    };
                }

                default:
                    throw new QuantityMeasurementException("Unsupported measurement category for conversion.");
            }
        }

        public QuantityDto AddQuantities(QuantityDto firstQuantity, QuantityDto secondQuantity, string resultUnit)
        {
            if (firstQuantity == null) throw new ArgumentNullException(nameof(firstQuantity));
            if (secondQuantity == null) throw new ArgumentNullException(nameof(secondQuantity));
            if (firstQuantity.Category != secondQuantity.Category)
            {
                throw new QuantityMeasurementException("Cannot add quantities of different categories.");
            }

            switch (firstQuantity.Category)
            {
                case MeasurementCategory.Length:
                {
                    LengthUnit u1 = ParseLengthUnit(firstQuantity.Unit);
                    LengthUnit u2 = ParseLengthUnit(secondQuantity.Unit);
                    LengthUnit ur = ParseLengthUnit(resultUnit);

                    QuantityLength result = domainService.AddQuantities(
                        firstQuantity.Value, u1, secondQuantity.Value, u2, ur);

                    return new QuantityDto
                    {
                        Category = MeasurementCategory.Length,
                        Unit = resultUnit,
                        Value = result.Value
                    };
                }

                case MeasurementCategory.Weight:
                {
                    WeightUnit u1 = ParseWeightUnit(firstQuantity.Unit);
                    WeightUnit u2 = ParseWeightUnit(secondQuantity.Unit);
                    WeightUnit ur = ParseWeightUnit(resultUnit);

                    QuantityWeight result = domainService.AddWeightQuantities(
                        firstQuantity.Value, u1, secondQuantity.Value, u2, ur);

                    return new QuantityDto
                    {
                        Category = MeasurementCategory.Weight,
                        Unit = resultUnit,
                        Value = result.Value
                    };
                }

                case MeasurementCategory.Volume:
                {
                    VolumeUnit u1 = ParseVolumeUnit(firstQuantity.Unit);
                    VolumeUnit u2 = ParseVolumeUnit(secondQuantity.Unit);
                    VolumeUnit ur = ParseVolumeUnit(resultUnit);

                    Quantity<VolumeUnit> result = domainService.AddVolumeQuantities(
                        firstQuantity.Value, u1, secondQuantity.Value, u2, ur);

                    return new QuantityDto
                    {
                        Category = MeasurementCategory.Volume,
                        Unit = resultUnit,
                        Value = result.Value
                    };
                }

                case MeasurementCategory.Temperature:
                    throw new QuantityMeasurementException("Addition is not supported for temperature.");

                default:
                    throw new QuantityMeasurementException("Unsupported measurement category for addition.");
            }
        }

        public QuantityDto SubtractQuantities(QuantityDto firstQuantity, QuantityDto secondQuantity, string resultUnit)
        {
            if (firstQuantity == null) throw new ArgumentNullException(nameof(firstQuantity));
            if (secondQuantity == null) throw new ArgumentNullException(nameof(secondQuantity));
            if (firstQuantity.Category != secondQuantity.Category)
            {
                throw new QuantityMeasurementException("Cannot subtract quantities of different categories.");
            }

            switch (firstQuantity.Category)
            {
                case MeasurementCategory.Length:
                {
                    LengthUnit u1 = ParseLengthUnit(firstQuantity.Unit);
                    LengthUnit u2 = ParseLengthUnit(secondQuantity.Unit);
                    LengthUnit ur = ParseLengthUnit(resultUnit);

                    Quantity<LengthUnit> result = domainService.SubtractLength(
                        firstQuantity.Value, u1, secondQuantity.Value, u2, ur);

                    return new QuantityDto
                    {
                        Category = MeasurementCategory.Length,
                        Unit = resultUnit,
                        Value = result.Value
                    };
                }

                case MeasurementCategory.Weight:
                {
                    WeightUnit u1 = ParseWeightUnit(firstQuantity.Unit);
                    WeightUnit u2 = ParseWeightUnit(secondQuantity.Unit);
                    WeightUnit ur = ParseWeightUnit(resultUnit);

                    Quantity<WeightUnit> result = domainService.SubtractWeight(
                        firstQuantity.Value, u1, secondQuantity.Value, u2, ur);

                    return new QuantityDto
                    {
                        Category = MeasurementCategory.Weight,
                        Unit = resultUnit,
                        Value = result.Value
                    };
                }

                case MeasurementCategory.Volume:
                {
                    VolumeUnit u1 = ParseVolumeUnit(firstQuantity.Unit);
                    VolumeUnit u2 = ParseVolumeUnit(secondQuantity.Unit);
                    VolumeUnit ur = ParseVolumeUnit(resultUnit);

                    Quantity<VolumeUnit> result = domainService.SubtractVolume(
                        firstQuantity.Value, u1, secondQuantity.Value, u2, ur);

                    return new QuantityDto
                    {
                        Category = MeasurementCategory.Volume,
                        Unit = resultUnit,
                        Value = result.Value
                    };
                }

                case MeasurementCategory.Temperature:
                    throw new QuantityMeasurementException("Subtraction is not supported for temperature.");

                default:
                    throw new QuantityMeasurementException("Unsupported measurement category for subtraction.");
            }
        }

        public double DivideQuantities(QuantityDto firstQuantity, QuantityDto secondQuantity)
        {
            if (firstQuantity == null) throw new ArgumentNullException(nameof(firstQuantity));
            if (secondQuantity == null) throw new ArgumentNullException(nameof(secondQuantity));
            if (firstQuantity.Category != secondQuantity.Category)
            {
                throw new QuantityMeasurementException("Cannot divide quantities of different categories.");
            }

            switch (firstQuantity.Category)
            {
                case MeasurementCategory.Length:
                {
                    LengthUnit u1 = ParseLengthUnit(firstQuantity.Unit);
                    LengthUnit u2 = ParseLengthUnit(secondQuantity.Unit);
                    return domainService.DivideLength(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                case MeasurementCategory.Weight:
                {
                    WeightUnit u1 = ParseWeightUnit(firstQuantity.Unit);
                    WeightUnit u2 = ParseWeightUnit(secondQuantity.Unit);
                    return domainService.DivideWeight(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                case MeasurementCategory.Volume:
                {
                    VolumeUnit u1 = ParseVolumeUnit(firstQuantity.Unit);
                    VolumeUnit u2 = ParseVolumeUnit(secondQuantity.Unit);
                    return domainService.DivideVolume(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                case MeasurementCategory.Temperature:
                    throw new QuantityMeasurementException("Division is not supported for temperature.");

                default:
                    throw new QuantityMeasurementException("Unsupported measurement category for division.");
            }
        }

        // ======= Unit parsing helpers =======

        private static LengthUnit ParseLengthUnit(string unit)
        {
            if (string.Equals(unit, "feet", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "foot", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "ft", StringComparison.OrdinalIgnoreCase))
            {
                return LengthUnit.Feet;
            }

            if (string.Equals(unit, "inch", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "inches", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "in", StringComparison.OrdinalIgnoreCase))
            {
                return LengthUnit.Inch;
            }

            if (string.Equals(unit, "yard", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "yards", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "yd", StringComparison.OrdinalIgnoreCase))
            {
                return LengthUnit.Yard;
            }

            if (string.Equals(unit, "centimeter", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "centimeters", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "cm", StringComparison.OrdinalIgnoreCase))
            {
                return LengthUnit.Centimeter;
            }

            throw new QuantityMeasurementException($"Unsupported length unit: {unit}");
        }

        private static WeightUnit ParseWeightUnit(string unit)
        {
            if (string.Equals(unit, "kilogram", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "kilograms", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "kg", StringComparison.OrdinalIgnoreCase))
            {
                return WeightUnit.Kilogram;
            }

            if (string.Equals(unit, "gram", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "grams", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "g", StringComparison.OrdinalIgnoreCase))
            {
                return WeightUnit.Gram;
            }

            if (string.Equals(unit, "pound", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "pounds", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "lb", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "lbs", StringComparison.OrdinalIgnoreCase))
            {
                return WeightUnit.Pound;
            }

            throw new QuantityMeasurementException($"Unsupported weight unit: {unit}");
        }

        private static VolumeUnit ParseVolumeUnit(string unit)
        {
            if (string.Equals(unit, "litre", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "liter", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "l", StringComparison.OrdinalIgnoreCase))
            {
                return VolumeUnit.Litre;
            }

            if (string.Equals(unit, "millilitre", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "milliliter", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "ml", StringComparison.OrdinalIgnoreCase))
            {
                return VolumeUnit.Millilitre;
            }

            if (string.Equals(unit, "gallon", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "gallons", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "gal", StringComparison.OrdinalIgnoreCase))
            {
                return VolumeUnit.Gallon;
            }

            throw new QuantityMeasurementException($"Unsupported volume unit: {unit}");
        }

        private static TemperatureUnit ParseTemperatureUnit(string unit)
        {
            if (string.Equals(unit, "celsius", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "c", StringComparison.OrdinalIgnoreCase))
            {
                return TemperatureUnit.Celsius;
            }

            if (string.Equals(unit, "fahrenheit", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "f", StringComparison.OrdinalIgnoreCase))
            {
                return TemperatureUnit.Fahrenheit;
            }

            if (string.Equals(unit, "kelvin", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unit, "k", StringComparison.OrdinalIgnoreCase))
            {
                return TemperatureUnit.Kelvin;
            }

            throw new QuantityMeasurementException($"Unsupported temperature unit: {unit}");
        }
    }
}