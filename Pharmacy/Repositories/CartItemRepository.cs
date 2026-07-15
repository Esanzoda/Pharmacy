using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface ICartItemRepository : IBaseRepository<CartItem>
{
    Task<bool> DeleteItemInCart(long customerId, long cartitemId);
    Task<CartItem?> GetCartItemByCustomerIdAndItemIdtemAsync(long customerId, long cartItemid);
    Task<CartItem?> GetItemByProductIdInCartAsync(long customerId,long productId);
}

public class CartItemRepository : BaseRepository<CartItem>, ICartItemRepository


{
    public CartItemRepository(AppDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<bool> DeleteItemInCart(long customerId, long cartitemId)
    {
        var entity = await DbContext.CartItems
            .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.Id == cartitemId);
        if (entity == null)
            return false;


        DbContext.CartItems.Remove(entity);
        return true;
    }


    public async Task<CartItem?> GetCartItemByCustomerIdAndItemIdtemAsync(long customerId, long cartItemid)
    {
        return await DbContext.CartItems
            .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.Id == cartItemid);
    }

    public async Task<CartItem?> GetItemByProductIdInCartAsync(long customerId,long productId)
    {
        return await DbContext.CartItems
            .FirstOrDefaultAsync(  x => x.ProductId == productId && x.CustomerId == customerId);
    }
}