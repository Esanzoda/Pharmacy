using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Purchase.Command;

public record AddItemToPurchaseCommand(long Id, PurchaseItemRequest Request) : IRequest<PurchaseResponse>;
public class AddItemToPurchaseHendler:PurchaseDiBase,IRequestHandler<AddItemToPurchaseCommand,PurchaseResponse>
{
    public AddItemToPurchaseHendler(IPurchaseRepository purchaseRepository) : base(purchaseRepository)
    {
    }
    

    public async Task<PurchaseResponse> Handle(AddItemToPurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await PurchaseRepository.GetByIdAsync(request.Id);
        if (purchase == null)
            throw new ResourseNotFoundException($"Purchase with this id not found");
        var product = await ProductRepository.GetByIdAsync(request.Request.ProductId);
        if (product == null)
            throw new ResourseNotFoundException($"Product not found");
        //add check for barcode

        product.Stock += request.Request.Quantity;
        await ProductRepository.UpdateAsync(product);
        await ProductRepository.SaveChangesAsync();

        var purchaseItem = Mapper.Map<PurchaseItem>(request.Request);
        purchaseItem.PurchaseId = purchase.Id;
        purchaseItem.TotalPrice = request.Request.Quantity * request.Request.PurchasePrice;

        await PurchaseItemRepository.CreateAsync(purchaseItem);
        await PurchaseItemRepository.SaveChangesAsync();

        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);

        await PurchaseRepository.UpdateAsync(purchase);
        await PurchaseRepository.SaveChangesAsync();

        return Mapper.Map<PurchaseResponse>(purchaseItem);
    }
}