using AutoMapper;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart;

public class CartDiBase
{
    protected readonly ICartRepository CartRepository;
    protected readonly ICartItemRepository CartItemRepository;
    protected readonly IProductRepository ProductRepository;
    protected readonly IMapper Mapper;

    public CartDiBase(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IProductRepository productRepository, IMapper mapper)
    {
        CartRepository = cartRepository;
        CartItemRepository = cartItemRepository;
        ProductRepository = productRepository;
        Mapper = mapper;
    }
}