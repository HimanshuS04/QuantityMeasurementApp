using System.Text.Json.Serialization;

namespace QuantityService.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MeasurementCategory { Length, Weight, Volume, Temperature }

    public class QuantityDto
    {
        public MeasurementCategory Category { get; set; }
        public string Unit { get; set; } = string.Empty;
        public double Value { get; set; }
    }
}