using AutoMapper;
using MediatR;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart.Command;

public record CreateCartCommand(CartRequest Request) : IRequest<CartResponse>;

public class CreateCartHendler : CartDiBase, IRequestHandler<CreateCartCommand, CartResponse>
{
    public CreateCartHendler(ICartRepository cartRepository, ICartItemRepository cartItemRepository,
        IProductRepository productRepository, IMapper mapper) : base(cartRepository, cartItemRepository,
        productRepository, mapper)
    {
    }

    public async Task<CartResponse> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var existingCart = await CartRepository.GetCartByCustomerId(request.Request.CustomerId);
        if (existingCart != null)
            return Mapper.Map<CartResponse>(existingCart);

        var cart = Mapper.Map<Models.Domain.Cart>(request.Request);
        cart.TotalAmount = 0;
        await CartRepository.CreateAsync(cart);
        await CartRepository.SaveChangesAsync();
        return Mapper.Map<CartResponse>(cart);
    }
}