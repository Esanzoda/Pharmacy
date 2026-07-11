using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Purchase.Query;
public record GetPurchaseByEmployeIdQuery(int EmployeeId,int Page,int PageSize):IRequest<List<PurchaseResponse>>;
public class GetPurchaseByEmployeId:PurchaseDiBase,IRequestHandler<GetPurchaseByEmployeIdQuery,List<PurchaseResponse>>
{
    public GetPurchaseByEmployeId(IPurchaseRepository purchaseRepository) : base(purchaseRepository)
    {
    }

    public async Task<List<PurchaseResponse>> Handle(GetPurchaseByEmployeIdQuery request, CancellationToken cancellationToken)
    {
        var purchase = await PurchaseRepository.GetPurchaseByEmployeId(request.EmployeeId, request.Page, request.PageSize);
        if (purchase == null)
        {
            throw new ResourseNotFoundException("Purchase not found");
        }
        return Mapper.Map<List<PurchaseResponse>>(purchase);
    }
}