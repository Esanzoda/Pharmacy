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

public class  PurchaseService : BaseService<Purchase, PurchaseRequest, PurchaseResponse>, IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IPurchaseItemRepository _purchaseItemRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IProductRepository _productRepository;

    public PurchaseService(IPurchaseRepository purchaseRepository, IPurchaseItemRepository purchaseItemRepository,
        IEmployeeRepository employeeRepository, IProductRepository productRepository, IMapper mapper)
        : base(purchaseRepository, mapper)
    {
        _purchaseRepository = purchaseRepository;
        _purchaseItemRepository = purchaseItemRepository;
        _employeeRepository = employeeRepository;
        _productRepository = productRepository;
    }

    public override async Task<PurchaseResponse> CreateAsync(PurchaseRequest purchaserequest)
    {
        var employee = await _employeeRepository.GetByIdAsync(purchaserequest.EmployeId);
        if (employee == null)
            throw new ResourseNotFoundExeption($"Employee not found");
        var purchase = Mapper.Map<Purchase>(purchaserequest);
        purchase.CreateAt = DateTime.UtcNow;
        purchase.TotalAmout = 0;
        await _purchaseRepository.CreateAsync(purchase);
        return Mapper.Map<PurchaseResponse>(purchase);
    }

    public async Task<PurchaseItemResponse> AddItemToPurchase(long purchaseId, PurchaseItemRequest purchaserequest)
    {
        var purchase = await _purchaseRepository.GetByIdAsync(purchaseId);
        if (purchase == null)
            throw new ResourseNotFoundExeption($"Purchase not found");
        var product = await _productRepository.GetByIdAsync(purchaserequest.ProductId);
        if (product == null)
            throw new ResourseNotFoundExeption($"Product not found");

        product.Stock += purchaserequest.Quantity;
        await _productRepository.UpdateAsync(product);
        var purchaseItem = Mapper.Map<PurchaseItem>(purchaserequest);
        purchaseItem.CreateAt = DateTime.UtcNow;
        purchaseItem.TotalPrice = purchaserequest.Quantity * purchaserequest.PurchasePrice;
        purchase.PurchaseItems.Add(purchaseItem);
        purchase.TotalAmout = purchase.PurchaseItems.Sum(item => item.TotalPrice);
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

        // purchase.PurchaseItems.Remove(purchaseItemToRemove);
        await _purchaseItemRepository.DeleteAsync(purchaseItemId);
        purchase.TotalAmout = purchase.PurchaseItems.Sum(item => item.TotalPrice);
        return Mapper.Map<PurchaseItemResponse>(purchaseItemToRemove);
    }
}