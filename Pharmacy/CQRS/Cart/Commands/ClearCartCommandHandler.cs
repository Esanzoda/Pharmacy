using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Cart.Commands;

public record ClearCartCommand(
    long CustomerId
) : IRequest<bool>;

public class ClearCartCommandHandler(
    IApplicationDbContext dbContext
) : IRequestHandler<ClearCartCommand, bool>
{
    public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken);

        if (cart == null)
        {
            throw new RecourseNotFoundException("Cart not found");
        }

        if (cart.CartItems.Count == 0)
        {
            throw new RecourseNotFoundException("Cart  is already empty");
        }

        cart.TotalAmount = 0;
        dbContext.CartItems
            .RemoveRange(cart.CartItems!);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}