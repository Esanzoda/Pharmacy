using MediatR;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Purchase.Query;

public record GetPurchaseByDateQuery(DateTime Date, int PageNumber, int PageSize) : IRequest<List<PurchaseResponse>>;
public class GetPurchaseByDateHendler:PurchaseDiBase,IRequestHandler<GetPurchaseByDateQuery,List<PurchaseResponse>>
{
    public GetPurchaseByDateHendler(IPurchaseRepository purchaseRepository) : base(purchaseRepository)
    {
    }

    public async Task<List<PurchaseResponse>> Handle(GetPurchaseByDateQuery request, CancellationToken cancellationToken)
    {
        var purchase=await PurchaseRepository.GetPurchaseByDate(request.Date, request.PageNumber, request.PageSize);
        return Mapper.Map<List<PurchaseResponse>>(purchase);
    }
}