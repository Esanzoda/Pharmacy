using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Purchase.Commands;

public record RemoveItemFromPurchaseCommand(
    long EmployeeId,
    long PurchaseId,
    long ItemId) : IRequest<PurchaseResponse>;

public class RemoveItemFromPurchaseCommandHandler(
    IApplicationDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<RemoveItemFromPurchaseCommand, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(RemoveItemFromPurchaseCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FindAsync(request.EmployeeId, cancellationToken);
        if (employee == null)
        {
            throw new RecourseNotFoundException("Employee not found");
        }

        var purchase = await dbContext.Purchases
            .Include(x => x.PurchaseItems)
            .FirstOrDefaultAsync(x => x.Id == request.PurchaseId, cancellationToken);
        if (purchase == null)
        {
            throw new RecourseNotFoundException("Purchase not found");
        }

        var purchaseItemToRemove = purchase.PurchaseItems.FirstOrDefault(x => x.Id == request.ItemId);
        if (purchaseItemToRemove == null)
        {
            throw new RecourseNotFoundException("Purchase item not found");
        }

        var product = await dbContext.Products
            .FindAsync(purchaseItemToRemove.ProductId, cancellationToken);
        if (product == null)
            throw new RecourseNotFoundException("Product not found");

        product.Stock -= purchaseItemToRemove.Quantity;

        purchase.PurchaseItems.Remove(purchaseItemToRemove);
        dbContext.PurchaseItems.Remove(purchaseItemToRemove);
        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<PurchaseResponse>(purchaseItemToRemove);
    }
}