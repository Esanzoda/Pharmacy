using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Purchase.Queries;

public record GetPurchaseByEmployeIdQuery(
    int EmployeeId,
    int Page,
    int PageSize
) : IRequest<List<PurchaseResponse>>;

public class GetPurchaseByEmployeIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<GetPurchaseByEmployeIdQuery, List<PurchaseResponse>>
{
    public async Task<List<PurchaseResponse>> Handle(GetPurchaseByEmployeIdQuery request,
        CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .Include(o => o.PurchaseItems)
            .Where(o => o.EmployeeId == request.EmployeeId)
            .OrderBy(o => o.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        if (purchase == null)
        {
            throw new ResourseNotFoundException("Purchase not found");
        }

        return mapper.Map<List<PurchaseResponse>>(purchase);
    }
}