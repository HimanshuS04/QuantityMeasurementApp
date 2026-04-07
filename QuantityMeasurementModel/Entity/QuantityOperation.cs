namespace QuantityMeasurementApp
{
    /// <summary>
    /// Main operations table row:
    /// category, operation type, inputs (first/second) and result.
    /// Audit is handled by DB trigger in QuantityOperationsAudit table.
    /// </summary>
    public class QuantityOperation
    {
        public int Id { get; set; }  // INT IDENTITY in DB

        public MeasurementCategory Category { get; set; }

        public string OperationType { get; set; } = string.Empty;
         /// <summary>
        ///  user id who performed this operation .
        /// </summary>
        public int? UserId { get; set; }

        // Input 1
        public double FirstValue { get; set; }
        public string FirstUnit { get; set; } = string.Empty;

        // Input 2 (optional)
        public double? SecondValue { get; set; }
        public string? SecondUnit { get; set; }

        // Result (optional)
        public double? ResultValue { get; set; }
        public string? ResultUnit { get; set; }
    }
}