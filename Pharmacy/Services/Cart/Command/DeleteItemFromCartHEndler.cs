using MediatR;
using Pharmasy.Exception;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart.Command;

public record DeleteItemFromCartCommand(long CustomerId, long ItemId) : IRequest<bool>;
public class DeleteItemFromCartHEndler:IRequestHandler<DeleteItemFromCartCommand,bool>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;

    public DeleteItemFromCartHEndler(ICartItemRepository cartItemRepository, ICartRepository cartRepository)
    {
        _cartItemRepository = cartItemRepository;
        _cartRepository = cartRepository;
    }

    public async Task<bool> Handle(DeleteItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartByCustomerId(request.CustomerId);
        if (cart == null)
        {
            throw new ResourseNotFoundException("Cart not found");
        }

        var item = await _cartItemRepository.DeleteAsync(request.ItemId);
        if (item != true)
        {
            throw new System.Exception("Cant't delete item in cart");
        }

        cart.CartItems.Remove(cart.CartItems.First(x => x.Id == request.ItemId));
        cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);
        await _cartRepository.UpdateAsync(cart);
        await _cartRepository.SaveChangesAsync();
        return true;
    }
}