using QuantityService.Models;

namespace QuantityService.Data
{
    public interface IQuantityOperationRepository
    {
        Task SaveAsync(QuantityOperation operation);
        Task<IReadOnlyList<QuantityOperation>> GetAllAsync();
        Task<IReadOnlyList<QuantityOperation>> GetByUserIdAsync(int userId);
        Task<IReadOnlyList<QuantityOperation>> GetByOperationTypeAsync(string operationType);
        Task<IReadOnlyList<QuantityOperation>> GetByCategoryAsync(MeasurementCategory category);
    }
}