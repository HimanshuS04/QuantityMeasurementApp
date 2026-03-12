using System;
using System.Collections.Generic;
using QuantityMeasurementApp;

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
    }
}