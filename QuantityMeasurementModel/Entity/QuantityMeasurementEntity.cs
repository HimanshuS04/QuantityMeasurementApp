using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Entity representing a quantity measurement operation for logging in the repository.
    /// </summary>
    public class QuantityMeasurementEntity
    {
        // NOTE: Properties must have public setters so System.Text.Json can deserialize them.
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }

        public string OperationType { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Optional measurement category (Length, Weight, Volume, Temperature) for this operation.
        /// </summary>
        public MeasurementCategory? Category { get; set; }

        /// <summary>
        /// Parameterless constructor required for JSON deserialization.
        /// </summary>
        public QuantityMeasurementEntity()
        {
        }

        // Convenience constructors used when logging from the service

        public QuantityMeasurementEntity(string operationType, string details, MeasurementCategory? category = null)
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
            OperationType = operationType;
            Details = details;
            HasError = false;
            ErrorMessage = null;
            Category = category;
        }

        public QuantityMeasurementEntity(string operationType, string details, string errorMessage, MeasurementCategory? category = null)
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
            OperationType = operationType;
            Details = details;
            HasError = true;
            ErrorMessage = errorMessage;
            Category = category;
        }

        /// <summary>
        /// Constructor used by the database repository when materializing from SQL.
        /// </summary>
        public QuantityMeasurementEntity(
            Guid id,
            DateTime timestamp,
            string operationType,
            string details,
            bool hasError,
            string? errorMessage,
            MeasurementCategory? category)
        {
            Id = id;
            Timestamp = timestamp;
            OperationType = operationType;
            Details = details;
            HasError = hasError;
            ErrorMessage = errorMessage;
            Category = category;
        }

        public override string ToString()
        {
            string categoryText = Category.HasValue ? Category.Value.ToString() : "None";
            if (HasError)
            {
                return $"[{Timestamp:u}] {OperationType} ({categoryText}) ERROR: {ErrorMessage} | {Details}";
            }

            return $"[{Timestamp:u}] {OperationType} ({categoryText}): {Details}";
        }
    }
}