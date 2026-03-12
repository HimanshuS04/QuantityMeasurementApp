namespace QuantityMeasurementApp
{
    public enum MeasurementCategory
    {
        Length,
        Weight,
        Volume,
        Temperature
    }

    /// <summary>
    /// DTO representing value + unit + category for UC15.
    /// Used between controller and DTO-based service.
    /// </summary>
    public class QuantityDto
    {
        public MeasurementCategory Category { get; set; }

        /// <summary>
        /// Unit name as a user-facing string. Examples:
        /// "feet", "inch", "yard", "centimeter",
        /// "kg", "g", "lb",
        /// "litre", "ml", "gallon",
        /// "celsius", "fahrenheit", "kelvin".
        /// </summary>
        public string Unit { get; set; } = string.Empty;

        public double Value { get; set; }

        public override string ToString()
        {
            return $"{Value} {Unit} ({Category})";
        }
    }
}