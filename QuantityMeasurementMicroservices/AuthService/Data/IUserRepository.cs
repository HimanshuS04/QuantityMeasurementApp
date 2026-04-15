using AuthService.Models;

namespace AuthService.Data
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task CreateAsync(User user);
    }
}