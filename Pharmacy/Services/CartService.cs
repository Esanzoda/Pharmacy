using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
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

public class CartService : BaseService<Models.Domain.Cart, CartRequest, CartResponse>, ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IDistributedCache _cache;

    public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository,
        IProductRepository productRepository, IDistributedCache distributedCache,
        IMapper mapper)
        : base(cartRepository, mapper, distributedCache)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
    }

    public override async Task<CartResponse> CreateAsync(CartRequest request)
    {
        var existingCart = await _cartRepository.GetCartByCustomerId(request.CustomerId);
        if (existingCart != null)
            return Mapper.Map<CartResponse>(existingCart);

        var cart = Mapper.Map<Models.Domain.Cart>(request);
        cart.TotalAmount = 0;
        await _cartRepository.CreateAsync(cart);
        await _cartRepository.SaveChangesAsync();
        return Mapper.Map<CartResponse>(cart);
    }


    public async Task<bool> ClearCartAsync(long customerId)
    {
        return await _cartRepository.ClearCartAsync(customerId);
    }
}