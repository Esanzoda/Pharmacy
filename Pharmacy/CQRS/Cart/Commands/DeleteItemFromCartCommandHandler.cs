using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Cart.Commands;

public record DeleteItemFromCartCommand(
    long CustomerId, 
    long ItemId
    ) : IRequest<bool>;

public class DeleteItemFromCartCommandHandler( IApplicationDbContext dbContext)
    : IRequestHandler<DeleteItemFromCartCommand, bool>
{
    public async Task<bool> Handle(DeleteItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken);
        if (cart == null)
        {
            throw new ResourseNotFoundException("Cart not found");
        }

        var item = cart.CartItems.FirstOrDefault(x => x.Id == request.ItemId);
        if (item is null)
        {
            throw new System.Exception("Cart item not found");
        }

        cart.CartItems.Remove(item);
        dbContext.CartItems.Remove(item);

        cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}