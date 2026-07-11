using MediatR;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Purchase.Query;
public record GetAllPurchaseQuery(int Page,int PageSiza):IRequest<List<PurchaseResponse>>;
public class GetAllPurchaseHendler:PurchaseDiBase,IRequestHandler<GetAllPurchaseQuery,List<PurchaseResponse>>
{
    public GetAllPurchaseHendler(IPurchaseRepository purchaseRepository) : base(purchaseRepository)
    {
    }

    public async Task<List<PurchaseResponse>> Handle(GetAllPurchaseQuery request, CancellationToken cancellationToken)
    {
        var purchase =await  PurchaseRepository.GetAllByPaginationAsync(request.Page, request.PageSiza);
        return Mapper.Map<List<PurchaseResponse>>(purchase);
    }
}