using System;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementService : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository quantityMeasurementRepository;

        public QuantityMeasurementService(IQuantityMeasurementRepository quantityMeasurementRepository)
        {
            this.quantityMeasurementRepository = quantityMeasurementRepository
                                                 ?? throw new ArgumentNullException(nameof(quantityMeasurementRepository));
        }

        // Helper to log operations
        private void Log(string operationType, string details)
        {
            quantityMeasurementRepository.Save(new QuantityMeasurementEntity(operationType, details));
        }

        private void LogError(string operationType, string details, Exception ex)
        {
            quantityMeasurementRepository.Save(new QuantityMeasurementEntity(operationType, details, ex.Message));
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
                Log(op, $"{details}, result={result}");
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={result}");
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={result}");
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={converted}");
                return converted;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={result.Value} {result.Unit}");
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={result.Value} {result.Unit}");
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, ratio={ratio}");
                return ratio;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={result}");
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={converted}");
                return converted;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={result.Value} {result.Unit}");
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={result.Value} {result.Unit}");
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, ratio={ratio}");
                return ratio;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={result}");
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={converted.Value}");
                return converted.Value;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={result.Value} {result.Unit}");
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={result.Value} {result.Unit}");
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, ratio={ratio}");
                return ratio;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={result}");
                return result;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
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
                Log(op, $"{details}, result={converted.Value}");
                return converted.Value;
            }
            catch (Exception ex)
            {
                LogError(op, details, ex);
                throw new QuantityMeasurementException("Error in temperature conversion.", ex);
            }
        }
    }
}