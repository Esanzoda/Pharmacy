using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Purchase.Command;

public record UpdatePurchaseCommand(long Id, PurchaseRequest Request) : IRequest<PurchaseResponse>;
public class UpdatePurchaseHendler:PurchaseDiBase,IRequestHandler<UpdatePurchaseCommand,PurchaseResponse>
{
    public UpdatePurchaseHendler(IPurchaseRepository purchaseRepository) : base(purchaseRepository)
    {
    }

    public async Task<PurchaseResponse> Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await PurchaseRepository.GetByIdAsync(request.Id);
        if (purchase == null)
        {
            throw new ResourseNotFoundException("Purchase not found");
        }

        purchase.EmployeeId = request.Request.EmployeeId;
        await PurchaseRepository.UpdateAsync(purchase);
        await PurchaseRepository.SaveChangesAsync();
        return Mapper.Map<PurchaseResponse>(purchase);
    }
}