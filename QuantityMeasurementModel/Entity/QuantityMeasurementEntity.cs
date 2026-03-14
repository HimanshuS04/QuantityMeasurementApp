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

        public QuantityMeasurementEntity(string operationType, string details)
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
            OperationType = operationType;
            Details = details;
            HasError = false;
            ErrorMessage = null;
        }

        public QuantityMeasurementEntity(string operationType, string details, string errorMessage)
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
            OperationType = operationType;
            Details = details;
            HasError = true;
            ErrorMessage = errorMessage;
        }
        public QuantityMeasurementEntity()
        {
            
        }
        public override string ToString()
        {
            if (HasError)
            {
                return $"[{Timestamp:u}] {OperationType} ERROR: {ErrorMessage} | {Details}";
            }

            return $"[{Timestamp:u}] {OperationType}: {Details}";
        }
    }
}