using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface ICartItemRepository : IBaseRepository<CartItem>
{
  
    Task<bool> ClearCartAsync(long cartId);
    Task<bool> UpdateQuantityCartItemAsync(long cartId,long cartItemId, int quantity);
    Task<CartItem?> GetProductWhithProductIdInCartItemAsync( long productId);
  
}
public class  CartItemRepository:BaseRepository<CartItem>, ICartItemRepository


{
    public CartItemRepository(AppDbContext dbContext) 
        : base(dbContext)
    {
    }
//dublicate method
    public async Task<bool> DeleteByCartIdAndProductIdAsync(long cartId, long cartitemId)
    {
        var entity =await DbContext.CartItems
            .FirstOrDefaultAsync(x => x.CartId == cartId && x.Id == cartitemId);
        if (entity == null)
        {
            return false;
        }

        DbContext.CartItems.Remove(entity);
        await  DbContext.SaveChangesAsync();
        return true;
        
    }

    public async Task<bool> ClearCartAsync(long cartId)
    {
        var items = await DbContext.CartItems
            .Where(x => x.CartId == cartId)
            .ToListAsync();

        if (!items.Any())
            return false;

        DbContext.CartItems.RemoveRange(items);
        await DbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateQuantityCartItemAsync(long cartId,long cartItemId, int quantity)
    {
        var entity = await  DbContext.CartItems
            .FirstOrDefaultAsync(x => x.CartId == cartId && x.Id == cartItemId);
        if (entity == null)
            return false;
        
        entity.Quantity = quantity;
        await DbContext.SaveChangesAsync();
        return true;
    }
    
    public async Task<CartItem?> GetProductWhithProductIdInCartItemAsync(long productId)
    {
        return await DbContext.CartItems
            .FirstOrDefaultAsync(x => x.ProductId == productId);
    }

}