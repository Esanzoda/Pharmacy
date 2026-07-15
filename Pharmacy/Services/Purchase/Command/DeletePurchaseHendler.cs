using MediatR;
using Pharmasy.Exception;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Purchase.Command;

public record DeletePurchaseCommand(long Id): IRequest<bool>;
public class DeletePurchaseHendler:PurchaseDiBase,IRequestHandler<DeletePurchaseCommand,bool>
{
    public DeletePurchaseHendler(IPurchaseRepository purchaseRepository) : base(purchaseRepository)
    {
    }

    public async Task<bool> Handle(DeletePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await PurchaseRepository.DeleteAsync(request.Id);
        if (purchase is false)
        {
            throw new ResourseNotFoundException($"Purchase with id {request.Id} not found");
        }

        await PurchaseRepository.SaveChangesAsync();
        return true;
    }
}