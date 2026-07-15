using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart.Command;

public record ClearCartCommand(long CustomerId) : IRequest<bool>;
public class ClearCartHendler:CartDiBase,IRequestHandler<ClearCartCommand,bool>
{
    public ClearCartHendler(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IProductRepository productRepository, IMapper mapper) : base(cartRepository, cartItemRepository, productRepository, mapper)
    {
    }

    public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await CartRepository.GetCartByCustomerId(request.CustomerId);
        if (cart == null)
        {
            throw new ResourseNotFoundException("Cart not found");
        }
        var cleared=await CartRepository.ClearCartAsync(cart.CustomerId);
        if (!cleared)
        {
            throw new ResourseNotFoundException("Cart  is alredy empty");
        }

        cart.TotalAmount = 0;
        await CartRepository.UpdateAsync(cart);
        await CartRepository.SaveChangesAsync();
      return  await CartRepository.DeleteAsync(request.CustomerId);
    }
}