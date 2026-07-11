using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface ICartRepository : IBaseRepository<Cart>
{
    Task<bool> ClearCartAsync(long customerId);
    Task<Cart?> GetCartByCustomerId(long customerId);
}

public class CartRepository : BaseRepository<Cart>, ICartRepository
{
    public CartRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Cart?> GetCartByCustomerId(long customerId)
    {
        return await DbContext.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);
    }

    public async Task<bool> ClearCartAsync(long customerId)
    {
        var items = await DbContext.CartItems
            .Where(x => x.CustomerId == customerId)
            .ToListAsync();

        if (!items.Any())
            return false;

        DbContext.CartItems.RemoveRange(items);
        return true;
    }
}