using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
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

public class PurchaseService : BaseService<Models.Domain.Purchase, PurchaseRequest, PurchaseResponse>, IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IPurchaseItemRepository _purchaseItemRepository;
    private readonly IEmployeeService _employeeService;
    private readonly IProductRepository _productRepository;
    private readonly IDistributedCache _cache;

    public PurchaseService(IPurchaseRepository purchaseRepository, IPurchaseItemRepository purchaseItemRepository,
        IDistributedCache distributedCache,
        IEmployeeService employeeService, IProductRepository productRepository, IMapper mapper)
        : base(purchaseRepository, mapper, distributedCache)
    {
        _purchaseRepository = purchaseRepository;
        _purchaseItemRepository = purchaseItemRepository;
        _employeeService = employeeService;
        _productRepository = productRepository;
        _cache = distributedCache;
    }

    public override async Task<PurchaseResponse> CreateAsync(PurchaseRequest purchaserequest)
    {
        var employee = await _employeeService.GetByIdAsync(purchaserequest.EmployeeId, CancellationToken.None);
        if (employee == null)
        {
            throw new ResourseNotFoundException($"Employee with this id  not found");
        }

        var purchase = Mapper.Map<Models.Domain.Purchase>(purchaserequest);
        await _purchaseRepository.CreateAsync(purchase);
        await _purchaseRepository.SaveChangesAsync();

        foreach (var item in purchaserequest.PurchaseItems)
        {
            var product = await _productRepository.GetProductByBarcodeAsync(item.Barcode);
            if (product == null)
            {
                throw new ResourseNotFoundException($"Product whith this barcode not found");
            }

            var purchaseItem = Mapper.Map<PurchaseItem>(item);
            purchaseItem.PurchaseId = purchase.Id;
            purchaseItem.TotalPrice = item.Quantity * item.PurchasePrice;

            await _purchaseItemRepository.CreateAsync(purchaseItem);
            await _purchaseItemRepository.SaveChangesAsync();

            product.Stock += item.Quantity;
            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();


            purchase.TotalAmount = purchase.PurchaseItems.Sum(x => x.TotalPrice);
            await _purchaseRepository.UpdateAsync(purchase);
            await _purchaseRepository.SaveChangesAsync();
        }

        return Mapper.Map<PurchaseResponse>(purchase);
    }

    public async Task<PurchaseItemResponse> AddItemToPurchase(long purchaseId, PurchaseItemRequest purchaserequest)
    {
        var purchase = await _purchaseRepository.GetByIdAsync(purchaseId);
        if (purchase == null)
            throw new ResourseNotFoundException($"Purchase with this id not found");
        var product = await _productRepository.GetByIdAsync(purchaserequest.ProductId);
        if (product == null)
            throw new ResourseNotFoundException($"Product not found");
        //add check for barcode

        product.Stock += purchaserequest.Quantity;
        await _productRepository.UpdateAsync(product);
        await _productRepository.SaveChangesAsync();

        var purchaseItem = Mapper.Map<PurchaseItem>(purchaserequest);
        purchaseItem.PurchaseId = purchase.Id;
        purchaseItem.TotalPrice = purchaserequest.Quantity * purchaserequest.PurchasePrice;

        await _purchaseItemRepository.CreateAsync(purchaseItem);
        await _purchaseItemRepository.SaveChangesAsync();

        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);

        await _purchaseRepository.UpdateAsync(purchase);
        await _purchaseRepository.SaveChangesAsync();

        return Mapper.Map<PurchaseItemResponse>(purchaseItem);
    }

    public async Task<PurchaseItemResponse> RemoveItemFromPurchase(long purchaseId, long purchaseItemId)
    {
        var purchase = await _purchaseRepository.GetByIdAsync(purchaseId);
        if (purchase == null)
            throw new ResourseNotFoundException("Purchase not found");

        var purchaseItemToRemove = await _purchaseItemRepository.GetByIdAsync(purchaseItemId);
        if (purchaseItemToRemove == null)
            throw new ResourseNotFoundException("Purchase item not found");
        var product = await _productRepository.GetByIdAsync(purchaseItemToRemove.ProductId);
        if (product == null)
            throw new ResourseNotFoundException("Porduct not found");

        product.Stock -= purchaseItemToRemove.Quantity;
        await _productRepository.UpdateAsync(product);
        await _productRepository.SaveChangesAsync();

        purchase.PurchaseItems.Remove(purchaseItemToRemove);
        await _purchaseItemRepository.DeleteAsync(purchaseItemId);
        await _purchaseRepository.SaveChangesAsync();

        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);
        await _purchaseRepository.UpdateAsync(purchase);
        await _purchaseItemRepository.SaveChangesAsync();

        return Mapper.Map<PurchaseItemResponse>(purchaseItemToRemove);
    }
}