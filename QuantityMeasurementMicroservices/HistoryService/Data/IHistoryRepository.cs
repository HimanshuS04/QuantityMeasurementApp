using HistoryService.Models;

namespace HistoryService.Data
{
    public interface IHistoryRepository
    {
        Task SaveAsync(OperationHistory operation);
        Task<IReadOnlyList<OperationHistory>> GetByUserIdAsync(int userId);
        Task<IReadOnlyList<OperationHistory>> GetAllAsync();
    }
}