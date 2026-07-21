using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Purchase.Commands;

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