using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Purchase.Queries;

public record GetPurchaseByDateQuery(
    DateTime Date, 
    int PageNumber, 
    int PageSize) : IRequest<List<PurchaseResponse>>;
public class GetPurchaseByDateHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
    ):IRequestHandler<GetPurchaseByDateQuery,List<PurchaseResponse>>
{

    public async Task<List<PurchaseResponse>> Handle(GetPurchaseByDateQuery request, CancellationToken cancellationToken)
    {
        var purchase=await dbContext.Purchases
            .Include(o => o.PurchaseItems)
            .Where(x=>x.CreatedAt == request.Date)  
            .OrderBy(o => o.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return mapper.Map<List<PurchaseResponse>>(purchase);
    }
}