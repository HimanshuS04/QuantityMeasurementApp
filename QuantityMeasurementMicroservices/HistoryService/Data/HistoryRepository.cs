using Microsoft.EntityFrameworkCore;
using HistoryService.Models;

namespace HistoryService.Data
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly HistoryDbContext dbContext;

        public HistoryRepository(HistoryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveAsync(OperationHistory operation)
        {
            dbContext.OperationHistories.Add(operation);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<OperationHistory>> GetByUserIdAsync(int userId)
        {
            return await dbContext.OperationHistories
                .AsNoTracking()
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<OperationHistory>> GetAllAsync()
        {
            return await dbContext.OperationHistories.AsNoTracking().ToListAsync();
        }
    }
}