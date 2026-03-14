// QuantityMeasurementRepository/IQuantityMeasurementRepository.cs
namespace QuantityMeasurementApp
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        IReadOnlyList<QuantityMeasurementEntity> GetAll();
    }
}