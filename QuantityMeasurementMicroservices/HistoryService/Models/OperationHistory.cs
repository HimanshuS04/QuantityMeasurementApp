using System.Text.Json.Serialization;

namespace HistoryService.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OperationCategory
    {
        Length,
        Weight,
        Volume,
        Temperature
    }

    public class OperationHistory
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public OperationCategory Category { get; set; }
        public string OperationType { get; set; } = string.Empty;
        public double FirstValue { get; set; }
        public string FirstUnit { get; set; } = string.Empty;
        public double? SecondValue { get; set; }
        public string? SecondUnit { get; set; }
        public double? ResultValue { get; set; }
        public string? ResultUnit { get; set; }
    }
}