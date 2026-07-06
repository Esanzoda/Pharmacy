using AutoMapper;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface ICartService :
    IBaseService<CartRequest, CartResponse>
{
    Task<bool> ClearCartAsync(long customerId);
}

public class CartService : BaseService<Cart, CartRequest, CartResponse>, ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository,
        IProductRepository productRepository,
        IMapper mapper)
        : base(cartRepository, mapper)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
    }

    public override async Task<CartResponse> CreateAsync(CartRequest request)
    {
        var existingCart = await _cartRepository.GetCartByCustomerId(request.CistomerId);
        if (existingCart != null)
            return Mapper.Map<CartResponse>(existingCart);

        var cart = Mapper.Map<Cart>(request);
        cart.TotalAmout = 0;
        await _cartRepository.CreateAsync(cart);
        await _cartRepository.SavechangesAsync();
        return Mapper.Map<CartResponse>(cart);
    }


    public async Task<bool> ClearCartAsync(long customerId)
    {
        return await _cartRepository.ClearCartAsync(customerId);
    }
}