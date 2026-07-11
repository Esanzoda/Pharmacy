using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Purchase.Query;
public record GetPurchaseBuIdQuery(long Id):IRequest<PurchaseResponse>;
public class GetPurchaseBuIdHendler:PurchaseDiBase,IRequestHandler<GetPurchaseBuIdQuery,PurchaseResponse>
{
    public GetPurchaseBuIdHendler(IPurchaseRepository purchaseRepository) : base(purchaseRepository)
    {
    }

    public async Task<PurchaseResponse> Handle(GetPurchaseBuIdQuery request, CancellationToken cancellationToken)
    {
        var purchase = await PurchaseRepository.GetByIdAsync(request.Id);
        if (purchase == null)
        {
            throw new ResourseNotFoundException("Purchase not found");
        }
        return Mapper.Map<PurchaseResponse>(purchase);
    }
}