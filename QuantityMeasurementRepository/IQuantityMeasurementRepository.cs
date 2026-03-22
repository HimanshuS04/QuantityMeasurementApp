using System.Collections.Generic;

namespace QuantityMeasurementApp
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);

        IReadOnlyList<QuantityMeasurementEntity> GetAll();

        // === UC16: additional query and management methods ===

        /// <summary>
        /// Returns all measurements for a given operation type (e.g., "ADD_LENGTH").
        /// </summary>
        IReadOnlyList<QuantityMeasurementEntity> GetByOperationType(string operationType);

        /// <summary>
        /// Returns all measurements for a given measurement category (Length, Weight, Volume, Temperature).
        /// </summary>
        IReadOnlyList<QuantityMeasurementEntity> GetByMeasurementCategory(MeasurementCategory category);

        /// <summary>
        /// Returns the total number of stored measurements.
        /// </summary>
        int GetTotalCount();

        /// <summary>
        /// Deletes all measurements from the underlying store.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Returns pool statistics or general repository status for monitoring.
        /// </summary>
        string GetPoolStatistics();

        /// <summary>
        /// Releases any resources held by the repository (e.g., DB connections).
        /// </summary>
        void ReleaseResources();
    }
}