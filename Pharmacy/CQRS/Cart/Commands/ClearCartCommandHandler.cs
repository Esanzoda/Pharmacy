using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;

namespace Pharmasy.CQRS.Cart.Commands;

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
            throw new ResourseNotFoundException("Cart not found");
        }

        if (cart.CartItems.Count == 0)
        {
            throw new ResourseNotFoundException("Cart  is alredy empty");
        }

        cart.TotalAmount = 0;
        dbContext.CartItems
            .RemoveRange(cart.CartItems!);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}