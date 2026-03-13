using System;
using System.Collections.Generic;
using System.Linq;

namespace QuantityMeasurementApp
{
    public sealed class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static readonly Lazy<QuantityMeasurementCacheRepository> lazyInstance =
            new Lazy<QuantityMeasurementCacheRepository>(() => new QuantityMeasurementCacheRepository());

        public static QuantityMeasurementCacheRepository Instance => lazyInstance.Value;

        private readonly List<QuantityMeasurementEntity> entities;

        private QuantityMeasurementCacheRepository()
        {
            entities = new List<QuantityMeasurementEntity>();
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity != null)
            {
                entities.Add(entity);
            }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAll()
        {
            return entities.AsReadOnly();
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByOperationType(string operationType)
        {
            if (string.IsNullOrWhiteSpace(operationType))
            {
                return Array.Empty<QuantityMeasurementEntity>();
            }

            return entities
                .Where(e => string.Equals(e.OperationType, operationType, StringComparison.OrdinalIgnoreCase))
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByMeasurementCategory(MeasurementCategory category)
        {
            return entities
                .Where(e => e.Category.HasValue && e.Category.Value == category)
                .ToList()
                .AsReadOnly();
        }

        public int GetTotalCount()
        {
            return entities.Count;
        }

        public void DeleteAll()
        {
            entities.Clear();
        }

        public string GetPoolStatistics()
        {
            // No connection pool in the in-memory repository.
            return $"In-memory cache repository. Stored items: {entities.Count}. No connection pool.";
        }

        public void ReleaseResources()
        {
            // Nothing to release for in-memory repository.
        }
    }
}