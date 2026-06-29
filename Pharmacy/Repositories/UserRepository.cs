using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetByRoleAsync(Role role, int page, int pageSize);
    Task<List<User>> GetByNameAsync(string name, int page, int pageSize);
    Task<bool> EmailExistsAsync(string email);
}

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await DbContext.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower()
                                      && !x.IsDeleted);
    }

    public async Task<List<User>> GetByRoleAsync(Role role, int page, int pageSize)
    {
        return await DbContext.Users
            .Where(x => x.Role == role && !x.IsDeleted)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<User>> GetByNameAsync(string  name, int page, int pageSize)
    {
        return await DbContext.Users
            .Where(x => x.Name.ToLower().Contains(name.ToLower()) && !x.IsDeleted)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await DbContext.Users
            .AnyAsync(x => x.Email.ToLower() == email.ToLower() && !x.IsDeleted);
    }
}