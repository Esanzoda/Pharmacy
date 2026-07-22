using AutoMapper;
using MediatR;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Purchase.Commands;

public record UpdatePurchaseCommand(
    long Id,
    PurchaseRequest Request) : IRequest<PurchaseResponse>;

public class UpdatePurchaseHandler(
    IApplicationDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<UpdatePurchaseCommand, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .FindAsync(request.Id, cancellationToken);
        if (purchase == null)
        {
            throw new ResourseNotFoundException("Purchase not found");
        }

        purchase.EmployeeId = request.Request.EmployeeId;
        dbContext.Purchases.Update(purchase);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PurchaseResponse>(purchase);
    }
}