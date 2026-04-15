using Microsoft.EntityFrameworkCore;
using QuantityService.Models;

namespace QuantityService.Data
{
    public class QuantityOperationRepository : IQuantityOperationRepository
    {
        private readonly QuantityDbContext dbContext;

        public QuantityOperationRepository(QuantityDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveAsync(QuantityOperation operation)
        {
            dbContext.QuantityOperations.Add(operation);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<QuantityOperation>> GetAllAsync()
        {
            return await dbContext.QuantityOperations
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<QuantityOperation>> GetByUserIdAsync(int userId)
        {
            return await dbContext.QuantityOperations
                .AsNoTracking()
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<QuantityOperation>> GetByOperationTypeAsync(string operationType)
        {
            return await dbContext.QuantityOperations
                .AsNoTracking()
                .Where(o => string.Equals(o.OperationType, operationType, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<QuantityOperation>> GetByCategoryAsync(MeasurementCategory category)
        {
            return await dbContext.QuantityOperations
                .AsNoTracking()
                .Where(o => o.Category == category)
                .ToListAsync();
        }
    }
}