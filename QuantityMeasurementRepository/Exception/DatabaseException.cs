using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Represents errors that occur during database operations.
    /// </summary>
    public class DatabaseException : Exception
    {
        public DatabaseException(string message)
            : base(message)
        {
        }

        public DatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}