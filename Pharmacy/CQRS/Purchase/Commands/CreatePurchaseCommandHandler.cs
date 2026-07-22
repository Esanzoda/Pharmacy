using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Purchase.Commands;

public record CreatePurchaseCommand(
    PurchaseRequest Request
) : IRequest<PurchaseResponse>;

public class CreatePurchaseCommandHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<CreatePurchaseCommand, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FindAsync(request.Request.EmployeeId, cancellationToken);
        if (employee == null)
        {
            throw new ResourseNotFoundException($"Employee with this id  not found");
        }

        var purchase = mapper.Map<Models.Domain.Purchase>(request.Request);
        await dbContext.Purchases
            .AddAsync(purchase, cancellationToken);

        foreach (var item in request.Request.PurchaseItems)
        {
            var product = await dbContext.Products
                .FirstOrDefaultAsync(x => x.Barcode == item.Barcode, cancellationToken);
            if (product == null)
            {
                throw new ResourseNotFoundException($"Product whith this barcode not found");
            }

            var purchaseItem = mapper.Map<PurchaseItem>(item);
            purchaseItem.Purchase = purchase;
            purchaseItem.TotalPrice = item.Quantity * item.PurchasePrice;

            purchase.PurchaseItems.Add(purchaseItem);
            await dbContext.PurchaseItems
                .AddAsync(purchaseItem, cancellationToken);
            product.Stock += item.Quantity;
        }

        purchase.TotalAmount = purchase.PurchaseItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PurchaseResponse>(purchase);
    }
}