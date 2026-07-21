using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Purchase.Queries;

public record GetAllPurchaseQuery(
    int Page,
    int PageSiza) : IRequest<List<PurchaseResponse>>;

public class GetAllPurchaseQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetAllPurchaseQuery, List<PurchaseResponse>>
{
    public async Task<List<PurchaseResponse>> Handle(GetAllPurchaseQuery request, CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .Include(o => o.PurchaseItems)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSiza)
            .Take(request.PageSiza)
            .ToListAsync(cancellationToken);
        return mapper.Map<List<PurchaseResponse>>(purchase);
    }
}