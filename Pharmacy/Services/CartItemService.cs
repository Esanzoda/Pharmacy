using AutoMapper;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;
public interface ICartItemService
:IBaseService<CartItemRequest, CartItemResponse> 
{
    Task<bool> ClearCartAsync(long cartId);
}
public class  CartItemService:BaseService<CartItem,CartItemRequest,CartItemResponse>, ICartItemService
{
    private readonly ICartItemRepository _cartItemRepository;

    public CartItemService(ICartItemRepository cartItemRepository, IMapper mapper)
        : base(cartItemRepository, mapper)
    {
        _cartItemRepository = cartItemRepository;
    }
    
    

    public async Task<bool> ClearCartAsync(long cartId)
    {
        return await _cartItemRepository.ClearCartAsync(cartId);
    }

   
}