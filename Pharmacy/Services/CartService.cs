using AutoMapper;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface ICartService : 
    IBaseService<CartRequest, CartResponse> 
{
    
}
public class CartService:BaseService<Cart,CartRequest,CartResponse>,ICartService
{
    
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository, IMapper mapper)
        : base(cartRepository, mapper)
    {
        _cartRepository = cartRepository;
    }
}