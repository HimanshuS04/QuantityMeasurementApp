using System;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementService : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository quantityMeasurementRepository;

        public QuantityMeasurementService(IQuantityMeasurementRepository quantityMeasurementRepository)
        {
            this.quantityMeasurementRepository = quantityMeasurementRepository ?? throw new ArgumentNullException(nameof(quantityMeasurementRepository));
        }

        
        // Helper to log operations, now including optional measurement category
        private void Log(string operationType, string details, MeasurementCategory? category = null)
        {
            quantityMeasurementRepository.Save(new QuantityMeasurementEntity(operationType, details, category));
        }

        private void LogError(string operationType, string details, Exception ex, MeasurementCategory? category = null)
        {
            quantityMeasurementRepository.Save(new QuantityMeasurementEntity(operationType, details, ex.Message, category));
        }

        // =====================================================
        //  NEW: DTO-BASED PUBLIC API (implements interface)
        // =====================================================

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
                    return AreQuantitiesEqual(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                case MeasurementCategory.Weight:
                {
                    WeightUnit u1 = ParseWeightUnit(firstQuantity.Unit);
                    WeightUnit u2 = ParseWeightUnit(secondQuantity.Unit);
                    return AreWeightQuantitiesEqual(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                case MeasurementCategory.Volume:
                {
                    VolumeUnit u1 = ParseVolumeUnit(firstQuantity.Unit);
                    VolumeUnit u2 = ParseVolumeUnit(secondQuantity.Unit);
                    return AreVolumeQuantitiesEqual(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                case MeasurementCategory.Temperature:
                {
                    TemperatureUnit u1 = ParseTemperatureUnit(firstQuantity.Unit);
                    TemperatureUnit u2 = ParseTemperatureUnit(secondQuantity.Unit);
                    return AreTemperatureQuantitiesEqual(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                default:
                    throw new QuantityMeasurementException("Unsupported measurement category.");
            }
        }

        public QuantityDto ConvertQuantity(QuantityDto quantity, string targetUnit)
        {
            if (quantity == null) throw new ArgumentNullException(nameof(quantity));
            if (string.IsNullOrWhiteSpace(targetUnit))
                throw new ArgumentException("Target unit is required.", nameof(targetUnit));

            switch (quantity.Category)
            {
                case MeasurementCategory.Length:
                {
                    LengthUnit source = ParseLengthUnit(quantity.Unit);
                    LengthUnit target = ParseLengthUnit(targetUnit);
                    double resultValue = ConvertLength(quantity.Value, source, target);

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
                    double resultValue = ConvertWeight(quantity.Value, source, target);

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
                    double resultValue = ConvertVolume(quantity.Value, source, target);

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
                    double resultValue = ConvertTemperature(quantity.Value, source, target);

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

                    QuantityLength result = AddQuantities(
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

                    QuantityWeight result = AddWeightQuantities(
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

                    Quantity<VolumeUnit> result = AddVolumeQuantities(
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

                    Quantity<LengthUnit> result = SubtractLength(
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

                    Quantity<WeightUnit> result = SubtractWeight(
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

                    Quantity<VolumeUnit> result = SubtractVolume(
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
                    return DivideLength(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                case MeasurementCategory.Weight:
                {
                    WeightUnit u1 = ParseWeightUnit(firstQuantity.Unit);
                    WeightUnit u2 = ParseWeightUnit(secondQuantity.Unit);
                    return DivideWeight(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                case MeasurementCategory.Volume:
                {
                    VolumeUnit u1 = ParseVolumeUnit(firstQuantity.Unit);
                    VolumeUnit u2 = ParseVolumeUnit(secondQuantity.Unit);
                    return DivideVolume(firstQuantity.Value, u1, secondQuantity.Value, u2);
                }

                case MeasurementCategory.Temperature:
                    throw new QuantityMeasurementException("Division is not supported for temperature.");

                default:
                    throw new QuantityMeasurementException("Unsupported measurement category for division.");
            }
        }

        // Length

        public bool AreFeetMeasurementsEqual(double firstFeetValue, double secondFeetValue)
        {
            const string op = "ARE_FEET_EQUAL";
            string details = $"first={firstFeetValue} ft, second={secondFeetValue} ft";

            try
            {
                Feet firstFeet = new Feet(firstFeetValue);
                Feet secondFeet = new Feet(secondFeetValue);

                bool result = firstFeet.Equals(secondFeet);
                Log(op, $"{details}, result={result}",MeasurementCategory.Length);
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Length);
                throw new QuantityMeasurementException("Error in feet equality comparison.", ex);
            }
        }

        public bool AreInchMeasurementsEqual(double firstInchValue, double secondInchValue)
        {
            const string op = "ARE_INCH_EQUAL";
            string details = $"first={firstInchValue} in, second={secondInchValue} in";

            try
            {
                Inches firstInch = new Inches(firstInchValue);
                Inches secondInch = new Inches(secondInchValue);

                bool result = firstInch.Equals(secondInch);
                Log(op, $"{details}, result={result}",MeasurementCategory.Length);
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Length);
                throw new QuantityMeasurementException("Error in inch equality comparison.", ex);
            }
        }

        public bool AreQuantitiesEqual(double firstValue, LengthUnit firstUnit, double secondValue, LengthUnit secondUnit)
        {
            const string op = "ARE_LENGTH_EQUAL";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}";

            try
            {
                QuantityLength firstQuantity = new QuantityLength(firstValue, firstUnit);
                QuantityLength secondQuantity = new QuantityLength(secondValue, secondUnit);

                bool result = firstQuantity.Equals(secondQuantity);
                Log(op, $"{details}, result={result}",MeasurementCategory.Length);
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Length);
                throw new QuantityMeasurementException("Error in generic length equality comparison.", ex);
            }
        }

        public double ConvertLength(double value, LengthUnit sourceUnit, LengthUnit targetUnit)
        {
            const string op = "CONVERT_LENGTH";
            string details = $"value={value}, source={sourceUnit}, target={targetUnit}";

            try
            {
                double converted = QuantityLength.Convert(value, sourceUnit, targetUnit);
                Log(op, $"{details}, result={converted}",MeasurementCategory.Length);
                return converted;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Length);
                throw new QuantityMeasurementException("Error in length conversion.", ex);
            }
        }

        public QuantityLength AddQuantities(
            double firstValue,
            LengthUnit firstUnit,
            double secondValue,
            LengthUnit secondUnit,
            LengthUnit resultUnit)
        {
            const string op = "ADD_LENGTH";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}, resultUnit={resultUnit}";

            try
            {
                QuantityLength result = QuantityLength.Add(firstValue, firstUnit, secondValue, secondUnit, resultUnit);
                Log(op, $"{details}, result={result.Value} {result.Unit}",MeasurementCategory.Length);
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Length);
                throw new QuantityMeasurementException("Error in length addition.", ex);
            }
        }

        public Quantity<LengthUnit> SubtractLength(
            double firstValue,
            LengthUnit firstUnit,
            double secondValue,
            LengthUnit secondUnit,
            LengthUnit resultUnit)
        {
            const string op = "SUBTRACT_LENGTH";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}, resultUnit={resultUnit}";

            try
            {
                Quantity<LengthUnit> first = new Quantity<LengthUnit>(firstValue, firstUnit);
                Quantity<LengthUnit> second = new Quantity<LengthUnit>(secondValue, secondUnit);

                Quantity<LengthUnit> result = first.Subtract(second, resultUnit);
                Log(op, $"{details}, result={result.Value} {result.Unit}",MeasurementCategory.Length);
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Length);
                throw new QuantityMeasurementException("Error in length subtraction.", ex);
            }
        }

        public double DivideLength(
            double firstValue,
            LengthUnit firstUnit,
            double secondValue,
            LengthUnit secondUnit)
        {
            const string op = "DIVIDE_LENGTH";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}";

            try
            {
                Quantity<LengthUnit> first = new Quantity<LengthUnit>(firstValue, firstUnit);
                Quantity<LengthUnit> second = new Quantity<LengthUnit>(secondValue, secondUnit);

                double ratio = first.Divide(second);
                Log(op, $"{details}, ratio={ratio}",MeasurementCategory.Length);
                return ratio;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Length);
                throw new QuantityMeasurementException("Error in length division.", ex);
            }
        }

        // Weight

        public bool AreWeightQuantitiesEqual(double firstValue, WeightUnit firstUnit, double secondValue, WeightUnit secondUnit)
        {
            const string op = "ARE_WEIGHT_EQUAL";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}";

            try
            {
                QuantityWeight firstWeight = new QuantityWeight(firstValue, firstUnit);
                QuantityWeight secondWeight = new QuantityWeight(secondValue, secondUnit);

                bool result = firstWeight.Equals(secondWeight);
                Log(op, $"{details}, result={result}",MeasurementCategory.Weight);
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Weight);
                throw new QuantityMeasurementException("Error in weight equality comparison.", ex);
            }
        }

        public double ConvertWeight(double value, WeightUnit sourceUnit, WeightUnit targetUnit)
        {
            const string op = "CONVERT_WEIGHT";
            string details = $"value={value}, source={sourceUnit}, target={targetUnit}";

            try
            {
                double converted = QuantityWeight.Convert(value, sourceUnit, targetUnit);
                Log(op, $"{details}, result={converted}",MeasurementCategory.Weight);
                return converted;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Weight);
                throw new QuantityMeasurementException("Error in weight conversion.", ex);
            }
        }

        public QuantityWeight AddWeightQuantities(
            double firstValue,
            WeightUnit firstUnit,
            double secondValue,
            WeightUnit secondUnit,
            WeightUnit resultUnit)
        {
            const string op = "ADD_WEIGHT";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}, resultUnit={resultUnit}";

            try
            {
                QuantityWeight result = QuantityWeight.Add(firstValue, firstUnit, secondValue, secondUnit, resultUnit);
                Log(op, $"{details}, result={result.Value} {result.Unit}",MeasurementCategory.Weight);
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Weight);
                throw new QuantityMeasurementException("Error in weight addition.", ex);
            }
        }

        public Quantity<WeightUnit> SubtractWeight(
            double firstValue,
            WeightUnit firstUnit,
            double secondValue,
            WeightUnit secondUnit,
            WeightUnit resultUnit)
        {
            const string op = "SUBTRACT_WEIGHT";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}, resultUnit={resultUnit}";

            try
            {
                Quantity<WeightUnit> first = new Quantity<WeightUnit>(firstValue, firstUnit);
                Quantity<WeightUnit> second = new Quantity<WeightUnit>(secondValue, secondUnit);

                Quantity<WeightUnit> result = first.Subtract(second, resultUnit);
                Log(op, $"{details}, result={result.Value} {result.Unit}",MeasurementCategory.Weight);
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Weight);
                throw new QuantityMeasurementException("Error in weight subtraction.", ex);
            }
        }

        public double DivideWeight(
            double firstValue,
            WeightUnit firstUnit,
            double secondValue,
            WeightUnit secondUnit)
        {
            const string op = "DIVIDE_WEIGHT";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}";

            try
            {
                Quantity<WeightUnit> first = new Quantity<WeightUnit>(firstValue, firstUnit);
                Quantity<WeightUnit> second = new Quantity<WeightUnit>(secondValue, secondUnit);

                double ratio = first.Divide(second);
                Log(op, $"{details}, ratio={ratio}",MeasurementCategory.Weight);
                return ratio;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Weight);
                throw new QuantityMeasurementException("Error in weight division.", ex);
            }
        }

        // Volume

        public bool AreVolumeQuantitiesEqual(double firstValue, VolumeUnit firstUnit, double secondValue, VolumeUnit secondUnit)
        {
            const string op = "ARE_VOLUME_EQUAL";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}";

            try
            {
                Quantity<VolumeUnit> firstVolume = new Quantity<VolumeUnit>(firstValue, firstUnit);
                Quantity<VolumeUnit> secondVolume = new Quantity<VolumeUnit>(secondValue, secondUnit);

                bool result = firstVolume.Equals(secondVolume);
                Log(op, $"{details}, result={result}",MeasurementCategory.Volume);
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Volume);
                throw new QuantityMeasurementException("Error in volume equality comparison.", ex);
            }
        }

        public double ConvertVolume(double value, VolumeUnit sourceUnit, VolumeUnit targetUnit)
        {
            const string op = "CONVERT_VOLUME";
            string details = $"value={value}, source={sourceUnit}, target={targetUnit}";

            try
            {
                Quantity<VolumeUnit> volume = new Quantity<VolumeUnit>(value, sourceUnit);
                Quantity<VolumeUnit> converted = volume.ConvertTo(targetUnit);
                Log(op, $"{details}, result={converted.Value}",MeasurementCategory.Volume);
                return converted.Value;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Volume);
                throw new QuantityMeasurementException("Error in volume conversion.", ex);
            }
        }

        public Quantity<VolumeUnit> AddVolumeQuantities(
            double firstValue,
            VolumeUnit firstUnit,
            double secondValue,
            VolumeUnit secondUnit,
            VolumeUnit resultUnit)
        {
            const string op = "ADD_VOLUME";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}, resultUnit={resultUnit}";

            try
            {
                Quantity<VolumeUnit> firstVolume = new Quantity<VolumeUnit>(firstValue, firstUnit);
                Quantity<VolumeUnit> secondVolume = new Quantity<VolumeUnit>(secondValue, secondUnit);

                Quantity<VolumeUnit> result = firstVolume.Add(secondVolume, resultUnit);
                Log(op, $"{details}, result={result.Value} {result.Unit}",MeasurementCategory.Volume);
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Volume);
                throw new QuantityMeasurementException("Error in volume addition.", ex);
            }
        }

        public Quantity<VolumeUnit> SubtractVolume(
            double firstValue,
            VolumeUnit firstUnit,
            double secondValue,
            VolumeUnit secondUnit,
            VolumeUnit resultUnit)
        {
            const string op = "SUBTRACT_VOLUME";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}, resultUnit={resultUnit}";

            try
            {
                Quantity<VolumeUnit> firstVolume = new Quantity<VolumeUnit>(firstValue, firstUnit);
                Quantity<VolumeUnit> secondVolume = new Quantity<VolumeUnit>(secondValue, secondUnit);

                Quantity<VolumeUnit> result = firstVolume.Subtract(secondVolume, resultUnit);
                Log(op, $"{details}, result={result.Value} {result.Unit}",MeasurementCategory.Volume);
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Volume);
                throw new QuantityMeasurementException("Error in volume subtraction.", ex);
            }
        }

        public double DivideVolume(
            double firstValue,
            VolumeUnit firstUnit,
            double secondValue,
            VolumeUnit secondUnit)
        {
            const string op = "DIVIDE_VOLUME";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}";

            try
            {
                Quantity<VolumeUnit> firstVolume = new Quantity<VolumeUnit>(firstValue, firstUnit);
                Quantity<VolumeUnit> secondVolume = new Quantity<VolumeUnit>(secondValue, secondUnit);

                double ratio = firstVolume.Divide(secondVolume);
                Log(op, $"{details}, ratio={ratio}",MeasurementCategory.Volume);
                return ratio;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Volume);
                throw new QuantityMeasurementException("Error in volume division.", ex);
            }
        }

        // Temperature

        public bool AreTemperatureQuantitiesEqual(double firstValue, TemperatureUnit firstUnit, double secondValue, TemperatureUnit secondUnit)
        {
            const string op = "ARE_TEMPERATURE_EQUAL";
            string details = $"first={firstValue} {firstUnit}, second={secondValue} {secondUnit}";

            try
            {
                Quantity<TemperatureUnit> firstTemp = new Quantity<TemperatureUnit>(firstValue, firstUnit);
                Quantity<TemperatureUnit> secondTemp = new Quantity<TemperatureUnit>(secondValue, secondUnit);

                bool result = firstTemp.Equals(secondTemp);
                Log(op, $"{details}, result={result}",MeasurementCategory.Temperature);
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Temperature);
                throw new QuantityMeasurementException("Error in temperature equality comparison.", ex);
            }
        }

        public double ConvertTemperature(double value, TemperatureUnit sourceUnit, TemperatureUnit targetUnit)
        {
            const string op = "CONVERT_TEMPERATURE";
            string details = $"value={value}, source={sourceUnit}, target={targetUnit}";

            try
            {
                Quantity<TemperatureUnit> temp = new Quantity<TemperatureUnit>(value, sourceUnit);
                Quantity<TemperatureUnit> converted = temp.ConvertTo(targetUnit);
                Log(op, $"{details}, result={converted.Value}",MeasurementCategory.Temperature);
                return converted.Value;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex,MeasurementCategory.Temperature);
                throw new QuantityMeasurementException("Error in temperature conversion.", ex);
            }
        }
        // =========================================
        // NEW: unit parsing helpers (moved from DTO service)
        // =========================================

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