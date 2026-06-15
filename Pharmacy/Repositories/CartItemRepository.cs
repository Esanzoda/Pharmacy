using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.Repositories;

public interface ICartItemRepository : IBaseRepository<CartItem>
{
    Task<bool> DeleteByCartIdAndProductIdAsync(long cartId, long productId);
    Task<bool> ClearCartAsync(long cartId);
    Task<bool> UpdateQuantityCartItemAsync(long cartId,long cartItemId, int quantity);
    Task<CartItem?> GetByCartIdAndProductIdAsync(long cartId, long productId);
}
public class CartItemRepository:BaseRepository<CartItem>, ICartItemRepository


{
    public CartItemRepository(AppDbContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<bool> DeleteByCartIdAndProductIdAsync(long cartId, long cartitemId)
    {
        var entity =await _dbContext.CartItems
            .FirstOrDefaultAsync(x => x.CartId == cartId && x.Id == cartitemId);
        if (entity == null)
        {
            return false;
        }

        _dbContext.CartItems.Remove(entity);
        await  _dbContext.SaveChangesAsync();
        return true;
        
    }

    public async Task<bool> ClearCartAsync(long cartId)
    {
        var items = await _dbContext.CartItems
            .Where(x => x.CartId == cartId)
            .ToListAsync();

        if (!items.Any())
            return false;

        _dbContext.CartItems.RemoveRange(items);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateQuantityCartItemAsync(long cartId,long cartItemId, int quantity)
    {
        var entity = await  _dbContext.CartItems
            .FirstOrDefaultAsync(x => x.CartId == cartId && x.Id == cartItemId);
        if (entity == null)
            return false;
        
        entity.Quantity = quantity;
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public Task<CartItem?> GetByCartIdAndProductIdAsync(long cartId, long productId)
    {
        throw new NotImplementedException();
    }
}