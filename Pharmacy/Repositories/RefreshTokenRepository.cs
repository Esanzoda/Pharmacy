using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface IRefreshToken
{
    Task CreateAsync(RefreshToken refreshToken);


    Task<RefreshToken?> GetAsync(string token);


    Task UpdateAsync(RefreshToken refreshToken);
    public Task<int> SaveChangesAsync();
}

public class RefreshTokenRepository : IRefreshToken
{
    //save change in owne method 
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens
            .AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> GetAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}