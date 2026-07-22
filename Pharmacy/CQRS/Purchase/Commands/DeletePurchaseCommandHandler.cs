using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Commands;

public record DeletePurchaseCommand(
    long Id) : IRequest<bool>;

public class DeletePurchaseCommandHandler(
    IApplicationDbContext dbContext ) : IRequestHandler<DeletePurchaseCommand, bool>
{
    public async Task<bool> Handle(DeletePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .Include(x => x.PurchaseItems)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (purchase is null)
        {
            throw new ResourseNotFoundException($"Purchase with id {request.Id} not found");
        }

        dbContext.Purchases
            .Remove(purchase);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}