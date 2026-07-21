using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Purchase.Queries;

public record GetPurchaseBuIdQuery(
    long Id) : IRequest<PurchaseResponse>;

public class GetPurchaseBuIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetPurchaseBuIdQuery, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(GetPurchaseBuIdQuery request, CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .Include(x => x.PurchaseItems)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (purchase == null)
        {
            throw new ResourseNotFoundException("Purchase not found");
        }

        return mapper.Map<PurchaseResponse>(purchase);
    }
}