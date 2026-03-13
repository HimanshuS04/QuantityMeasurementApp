using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Entity representing a quantity measurement operation for logging in the repository.
    /// </summary>
    public class QuantityMeasurementEntity
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }

        public string OperationType { get; }
        public string Details { get; }
        public bool HasError { get; }
        public string? ErrorMessage { get; }

        /// <summary>
        /// Optional measurement category (Length, Weight, Volume, Temperature) for this operation.
        /// </summary>
        public MeasurementCategory? Category { get; }

        // Existing constructors kept for backward compatibility

        public QuantityMeasurementEntity(string operationType, string details)
            : this(operationType, details, category: null)
        {
        }

        public QuantityMeasurementEntity(string operationType, string details, string errorMessage)
            : this(operationType, details, errorMessage, category: null)
        {
        }

        // New overloads that accept an optional category

        public QuantityMeasurementEntity(string operationType, string details, MeasurementCategory? category)
            : this(Guid.NewGuid(), DateTime.UtcNow, operationType, details, hasError: false, errorMessage: null, category)
        {
        }

        public QuantityMeasurementEntity(string operationType, string details, string errorMessage, MeasurementCategory? category)
            : this(Guid.NewGuid(), DateTime.UtcNow, operationType, details, hasError: true, errorMessage: errorMessage, category)
        {
        }

        // Constructor used to materialize from the database

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