using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;
public interface ICartItemService
//:IBaseService<CartItemRequest, CartItemResponse> 
{
    Task<bool> AddToCartAsync(CartItemRequest request);
    Task<bool> RemoveFromCartAsync(long cartId, long productId);
    Task<bool> ClearCartAsync(long cartId);
    Task<bool> UpdateQuantityAsync(long cartId,long cartItemId, int quantity);   
}
public class CartItemService:ICartItemService
{
    private readonly ICartItemRepository _cartItemRepository;

    public CartItemService(ICartItemRepository cartItemRepository)
    {
        _cartItemRepository = cartItemRepository;
    }
    
    public async Task<bool> AddToCartAsync(CartItemRequest request)
    {
       
        var existingItem = await _cartItemRepository
            .GetByCartIdAndProductIdAsync(request.CartId, request.ProductId);

        if (existingItem != null)
        {
            
            existingItem.Quantity += request.Quantity;

            await _cartItemRepository.UpdateAsync(existingItem);
            return true;
        }

        
        var newItem = new CartItem
        {
            CartId = request.CartId,
            ProductId = request.ProductId,
            Quantity = request.Quantity
        };

        await _cartItemRepository.CreateAsync(newItem);

        return true;
    }

    public Task<bool> RemoveFromCartAsync(long cartId, long productId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ClearCartAsync(long cartId)
    {
        return await _cartItemRepository.ClearCartAsync(cartId);
    }

    public async Task<bool> UpdateQuantityAsync(long cartId,long cartItemId, int quantity)
    {
        return await _cartItemRepository
            .UpdateQuantityCartItemAsync(cartId, cartItemId, quantity);
    }
}