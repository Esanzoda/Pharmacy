using AutoMapper;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface IPurchaseService : IBaseService<PurchaseRequest, PurchaseResponse>
{
    Task<PurchaseItemResponse> AddItemToPurchase(long purchaseId, PurchaseItemRequest purchaserequest);
    Task<PurchaseItemResponse> RemoveItemFromPurchase(long purchaseId, long purchaseItemId);
}

public class PurchaseService : BaseService<Purchase, PurchaseRequest, PurchaseResponse>, IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IPurchaseItemRepository _purchaseItemRepository;
    private readonly IEmployeeService _employeeService;
    private readonly IProductRepository _productRepository;

    public PurchaseService(IPurchaseRepository purchaseRepository, IPurchaseItemRepository purchaseItemRepository,
        IEmployeeService employeeService, IProductRepository productRepository, IMapper mapper)
        : base(purchaseRepository, mapper)
    {
        _purchaseRepository = purchaseRepository;
        _purchaseItemRepository = purchaseItemRepository;
        _employeeService = employeeService;
        _productRepository = productRepository;
    }

    public override async Task<PurchaseResponse> CreateAsync(PurchaseRequest purchaserequest)
    {
        var employee = await _employeeService.GetByIdAsync(purchaserequest.EmployeId);
        if (employee == null)
            throw new ResourseNotFoundExeption($"Employee with this id  not found");
        var purchase = Mapper.Map<Purchase>(purchaserequest);
        await _purchaseRepository.CreateAsync(purchase);
        await _purchaseRepository.SavechangesAsync();

        foreach (var item in purchaserequest.PurchaseItems)
        {
            var product = await _productRepository.GetProductByBarcodeAsync(item.Barcode);
            if (product == null)
            {
                throw new ResourseNotFoundExeption($"Product whith this barcode not found");
            }

            var existingItem = await _purchaseItemRepository.GetByBarcodeAsync(purchase.Id, item.Barcode);
            if (existingItem == null)
            {
                existingItem.Quantity += item.Quantity;
                existingItem.TotalPrice = existingItem.PurchasePrice * existingItem.Quantity;
                await _purchaseItemRepository.UpdateAsync(existingItem);
                purchase.TotalAmout += purchase.PurchaseItems.Sum(x => x.TotalPrice);
                await _purchaseRepository.SavechangesAsync();
                await _purchaseRepository.UpdateAsync(purchase);
                await _purchaseItemRepository.SavechangesAsync();
                product.Stock += item.Quantity;
                await _productRepository.UpdateAsync(product);
                await _purchaseItemRepository.SavechangesAsync();
            }
            else
            {
                var purchaseItem = Mapper.Map<PurchaseItem>(item);
                purchaseItem.TotalPrice = item.Quantity * purchaseItem.PurchasePrice;
                await _purchaseItemRepository.CreateAsync(purchaseItem);
                await _purchaseItemRepository.SavechangesAsync();
                product.Stock += item.Quantity;
                await _productRepository.UpdateAsync(product);

                await _productRepository.SavechangesAsync();
                purchase.TotalAmout += purchase.PurchaseItems.Sum(x => x.TotalPrice);
                await _purchaseRepository.UpdateAsync(purchase);
                await _purchaseRepository.SavechangesAsync();
            }
        }

        return Mapper.Map<PurchaseResponse>(purchase);
    }

    public async Task<PurchaseItemResponse> AddItemToPurchase(long purchaseId, PurchaseItemRequest purchaserequest)
    {
        var purchase = await _purchaseRepository.GetByIdAsync(purchaseId);
        if (purchase == null)
            throw new ResourseNotFoundExeption($"Purchase with this id not found");
        var product = await _productRepository.GetByIdAsync(purchaserequest.ProductId);
        if (product == null)
            throw new ResourseNotFoundExeption($"Product not found");

        product.Stock += purchaserequest.Quantity;
        await _productRepository.UpdateAsync(product);
        await _productRepository.SavechangesAsync();
        var purchaseItem = Mapper.Map<PurchaseItem>(purchaserequest);

        purchaseItem.TotalPrice = purchaserequest.Quantity * purchaserequest.PurchasePrice;
        await _purchaseItemRepository.CreateAsync(purchaseItem);
        await _purchaseItemRepository.SavechangesAsync();
        purchase.PurchaseItems.Add(purchaseItem);
        purchase.TotalAmout = purchase.PurchaseItems.Sum(item => item.TotalPrice);
        await _purchaseRepository.UpdateAsync(purchase);
        await _purchaseRepository.SavechangesAsync();
        return Mapper.Map<PurchaseItemResponse>(purchaseItem);
    }

    public async Task<PurchaseItemResponse> RemoveItemFromPurchase(long purchaseId, long purchaseItemId)
    {
        var purchase = await _purchaseRepository.GetByIdAsync(purchaseId);
        if (purchase == null)
            throw new ResourseNotFoundExeption("Purchase not found");

        var purchaseItemToRemove = await _purchaseItemRepository.GetByIdAsync(purchaseItemId);
        if (purchaseItemToRemove == null)
            throw new ResourseNotFoundExeption("Purchase item not found");
        var product = await _productRepository.GetByIdAsync(purchaseItemToRemove.ProductId);
        if (product == null)
            throw new ResourseNotFoundExeption("Porduct not found");

        product.Stock -= purchaseItemToRemove.Quantity;
        await _productRepository.UpdateAsync(product);

        purchase.PurchaseItems.Remove(purchaseItemToRemove);
        await _purchaseItemRepository.DeleteAsync(purchaseItemId);
        purchase.TotalAmout = purchase.PurchaseItems.Sum(item => item.TotalPrice);
        await _purchaseRepository.UpdateAsync(purchase);
        await _purchaseItemRepository.SavechangesAsync();
        return Mapper.Map<PurchaseItemResponse>(purchaseItemToRemove);
    }
}