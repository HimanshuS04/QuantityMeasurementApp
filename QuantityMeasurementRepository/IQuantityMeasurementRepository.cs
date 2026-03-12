using System.Collections.Generic;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);

        IReadOnlyList<QuantityMeasurementEntity> GetAll();
    }
}