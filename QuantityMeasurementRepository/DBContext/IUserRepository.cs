using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task CreateAsync(User user);
    }
}