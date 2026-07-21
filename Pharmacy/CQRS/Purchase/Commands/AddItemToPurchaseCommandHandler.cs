using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Purchase.Commands;

public record AddItemToPurchaseCommand(
    long Id,
    PurchaseItemRequest Request) : IRequest<PurchaseResponse>;

public class AddItemToPurchaseCommandHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<AddItemToPurchaseCommand, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(AddItemToPurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .FindAsync(request.Id, cancellationToken);
        if (purchase == null)
            throw new ResourseNotFoundException($"Purchase with this id not found");
        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.Request.ProductId || x.Barcode == request.Request.Barcode,
                cancellationToken);
        if (product == null)
        {
            throw new ResourseNotFoundException($"Product not found");
        }

        product.Stock += request.Request.Quantity;
        var purchaseItem = mapper.Map<PurchaseItem>(request.Request);
        purchaseItem.PurchaseId = purchase.Id;
        purchaseItem.TotalPrice = request.Request.Quantity * request.Request.PurchasePrice;

        await dbContext.PurchaseItems
            .AddAsync(purchaseItem, cancellationToken);

        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<PurchaseResponse>(purchaseItem);
    }
}