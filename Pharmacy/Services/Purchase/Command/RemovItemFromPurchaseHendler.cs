using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Purchase.Command;

public record RemoveItemFromPurchaseCommandHandler(long EmployeeId,long PurchaseId, long ItemId): IRequest<PurchaseResponse>;
public class RemovItemFromPurchaseHendler:PurchaseDiBase,IRequestHandler<RemoveItemFromPurchaseCommandHandler,PurchaseResponse>
{
    public RemovItemFromPurchaseHendler(IPurchaseRepository purchaseRepository) : base(purchaseRepository)
    {
    }

    public async Task<PurchaseResponse> Handle(RemoveItemFromPurchaseCommandHandler request, CancellationToken cancellationToken)
    {
        var employee =await EmployeeService.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee == null)
        {
            throw new ResourseNotFoundException("Employee not found");
        }
        var purchase = await PurchaseRepository.GetByIdAsync(request.PurchaseId);
        if (purchase == null)
            throw new ResourseNotFoundException("Purchase not found");

        var purchaseItemToRemove = await PurchaseItemRepository.GetByIdAsync(request.ItemId);
        if (purchaseItemToRemove == null)
            throw new ResourseNotFoundException("Purchase item not found");
        var product = await ProductRepository.GetByIdAsync(purchaseItemToRemove.ProductId);
        if (product == null)
            throw new ResourseNotFoundException("Porduct not found");

        product.Stock -= purchaseItemToRemove.Quantity;
        await ProductRepository.UpdateAsync(product);
        await ProductRepository.SaveChangesAsync();

        purchase.PurchaseItems.Remove(purchaseItemToRemove);
        await PurchaseItemRepository.DeleteAsync(request.ItemId);
        await PurchaseRepository.SaveChangesAsync();

        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);
        await PurchaseRepository.UpdateAsync(purchase);
        await PurchaseItemRepository.SaveChangesAsync();

        return Mapper.Map<PurchaseResponse>(purchaseItemToRemove);
    }
    
}