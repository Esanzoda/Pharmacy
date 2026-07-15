using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Purchase.Command;

public record RemoveItemFromPurchaseCommand(long EmployeeId, long PurchaseId, long ItemId) : IRequest<PurchaseResponse>;

public class RemovItemFromPurchaseHendler : PurchaseDiBase, IRequestHandler<RemoveItemFromPurchaseCommand, PurchaseResponse>
{
    public RemovItemFromPurchaseHendler(IPurchaseRepository purchaseRepository) : base(purchaseRepository)
    {
    }

    public async Task<PurchaseResponse> Handle(RemoveItemFromPurchaseCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await EmployeeRepository.GetByIdAsync(request.EmployeeId);
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
        
        purchase.PurchaseItems.Remove(purchaseItemToRemove);
        await PurchaseItemRepository.DeleteAsync(request.ItemId);
        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);
        await PurchaseRepository.UpdateAsync(purchase);
        await ProductRepository.SaveChangesAsync();
        await PurchaseItemRepository.SaveChangesAsync();
        await PurchaseRepository.SaveChangesAsync();

        return Mapper.Map<PurchaseResponse>(purchaseItemToRemove);
    }
}