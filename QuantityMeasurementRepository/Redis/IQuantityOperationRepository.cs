using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Repository for structured quantity operations (inputs + result).
    /// Implemented with Redis as primary store and SQL Server as backing store.
    /// </summary>
    public interface IQuantityOperationRepository
    {
        Task SaveAsync(QuantityOperation operation);

        Task<IReadOnlyList<QuantityOperation>> GetAllAsync();

        Task<IReadOnlyList<QuantityOperation>> GetByOperationTypeAsync(string operationType);

        Task<IReadOnlyList<QuantityOperation>> GetByCategoryAsync(MeasurementCategory category);
        Task<IReadOnlyList<QuantityOperation>> GetByUserIdAsync(int userId);

    }
}