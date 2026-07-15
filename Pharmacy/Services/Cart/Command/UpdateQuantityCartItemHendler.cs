using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart.Command;

public record UpdateQuantityCartItemCommand(long CustomerId, long CartItemId, int Quantity)
    : IRequest<CartItemResponse>;

public class UpdateQuantityCartItemHendler : CartDiBase,
    IRequestHandler<UpdateQuantityCartItemCommand, CartItemResponse>
{
    public UpdateQuantityCartItemHendler(ICartRepository cartRepository, ICartItemRepository cartItemRepository,
        IProductRepository productRepository, IMapper mapper) : base(cartRepository, cartItemRepository,
        productRepository, mapper)
    {
    }

    public async Task<CartItemResponse> Handle(UpdateQuantityCartItemCommand request,
        CancellationToken cancellationToken)
    {
        var cart =await CartRepository.GetByIdAsync(request.CustomerId);
        if (cart == null)
        {
            throw new ResourseNotFoundException("Cart not found");
        }
        var cartItem =
            await CartItemRepository.GetCartItemByCustomerIdAndItemIdtemAsync(request.CustomerId, request.CartItemId);
        if (cartItem == null)
        {
            throw new ResourseNotFoundException("CartItem not found");
        }

        cartItem.Quantity = request.Quantity;
        cartItem.TotalPrice = cartItem.Price * request.Quantity;
        cart.TotalAmount = cart.CartItems.Sum(x=>x.TotalPrice);
        await CartItemRepository.UpdateAsync(cartItem);
        await CartItemRepository.SaveChangesAsync();
        return Mapper.Map<CartItemResponse>(cartItem);
    }
}