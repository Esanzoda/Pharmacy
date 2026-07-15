using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;


namespace Pharmasy.Services.Purchase.Command;

public record CreatePurchaseCommand(PurchaseRequest Request) : IRequest<PurchaseResponse>;

public class CreatePurchaseHendler : PurchaseDiBase, IRequestHandler<CreatePurchaseCommand, PurchaseResponse>
{
    public CreatePurchaseHendler(IPurchaseRepository purchaseRepository) : base(purchaseRepository)
    {
    }

    public async Task<PurchaseResponse> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
    {
        var employee = await EmployeeRepository.GetByIdAsync(request.Request.EmployeeId);
        if (employee == null)
        {
            throw new ResourseNotFoundException($"Employee with this id  not found");
        }

        var purchase = Mapper.Map<Models.Domain.Purchase>(request);
        await PurchaseRepository.CreateAsync(purchase);

        foreach (var item in request.Request.PurchaseItems)
        {
            var product = await ProductRepository.GetProductByBarcodeAsync(item.Barcode);
            if (product == null)
            {
                throw new ResourseNotFoundException($"Product whith this barcode not found");
            }

            var purchaseItem = Mapper.Map<PurchaseItem>(item);
            purchaseItem.Purchase = purchase;
            purchaseItem.TotalPrice = item.Quantity * item.PurchasePrice;

            purchase.PurchaseItems.Add(purchaseItem);
            product.Stock += item.Quantity;
            await ProductRepository.UpdateAsync(product);

            //  await PurchaseItemRepository.CreateAsync(purchaseItem);
            // await PurchaseItemRepository.SaveChangesAsync();
        }

        purchase.TotalAmount = purchase.PurchaseItems.Sum(x => x.TotalPrice);
        await PurchaseRepository.UpdateAsync(purchase);
        await PurchaseRepository.SaveChangesAsync(); //1
        await ProductRepository.SaveChangesAsync();


        return Mapper.Map<PurchaseResponse>(purchase);
    }
}