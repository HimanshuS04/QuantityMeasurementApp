using Microsoft.EntityFrameworkCore;
using AuthService.Models;

namespace AuthService.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext dbContext;

        public UserRepository(AuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task CreateAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }
    }
}