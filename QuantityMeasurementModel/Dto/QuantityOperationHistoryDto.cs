namespace QuantityMeasurementApp
{
    /// <summary>
    /// DTO for returning operation history (UC18).
    /// Represents a single operation performed by a user.
    /// </summary>
    public class QuantityOperationHistoryDto
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public MeasurementCategory Category { get; set; }

        public string OperationType { get; set; } = string.Empty;

        public double FirstValue { get; set; }
        public string FirstUnit { get; set; } = string.Empty;

        public double? SecondValue { get; set; }
        public string? SecondUnit { get; set; }

        public double? ResultValue { get; set; }
        public string? ResultUnit { get; set; }
    }
}