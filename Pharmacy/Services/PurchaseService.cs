using AutoMapper;
using Pharmasy.Data;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface IPurchaseService : IBaseService<PurchaseRequest, PurchaseResponse>
{
   
}
public class PurchaseService:BaseService<Purchase,PurchaseRequest,PurchaseResponse>, IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IPurchaseItemRepository _purchaseItemRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IProductRepository _productRepository;
    public PurchaseService(IPurchaseRepository purchaseRepository, IMapper mapper) 
        : base(purchaseRepository, mapper)
    {
        _purchaseRepository = purchaseRepository;
    }

    public override async Task<PurchaseResponse> CreateAsync(PurchaseRequest purchaserequest)
    {
        var employee = await _employeeRepository.GetByIdAsync(purchaserequest.EmployeId);
        if (employee == null)
            throw new ResourseNotFoundExeption($"Employee not found");
        var purchase= _mapper.Map<Purchase>(purchaserequest);
        purchase.CreateAt = DateTime.Now;
        purchase.TotalPrice = 0;
        await _purchaseRepository.CreateAsync(purchase);
        return _mapper.Map<PurchaseResponse>(purchase);
    }

    public async Task<PurchaseItemResponse> AddItemToPurchase(long purchaseId,PurchaseItemRequest purchaserequest)
    {
        var purchase = await _purchaseRepository.GetByIdAsync(purchaseId);
        if (purchase == null)
            throw new ResourseNotFoundExeption($"Purchase not found");
        var product = await _productRepository.GetByIdAsync(purchaserequest.ProductId);
        if (product == null)
            throw new ResourseNotFoundExeption($"Product not found");
        
         product.Stock+=purchaserequest.Quantity;
         await _productRepository.UpdateAsync(product);
         
        var purchaseItem = _mapper.Map<PurchaseItem>(purchaserequest);
        purchaseItem.CreateAt = DateTime.Now;
        purchaseItem.TotalPrice=purchaserequest.Quantity*purchaserequest.PurchasePrise;
        purchase.PurchaseItems.Add(purchaseItem);
        
        purchase.TotalPrice=purchase.PurchaseItems.Sum(item => item.TotalPrice);
        await _purchaseRepository.UpdateAsync(purchase);
        return _mapper.Map<PurchaseItemResponse>(purchaseItem);
    }

    public async Task<PurchaseItemResponse> RemoveItemFromPurchase(long purchaseId, long purchaseItemId)
    {
        var purchase = await _purchaseRepository.GetByIdAsync(purchaseId);
        if(purchase == null)
            throw new ResourseNotFoundExeption("Purchase not found");

        var purchaseItemToRemove = await _purchaseItemRepository.GetByIdAsync(purchaseItemId);
        if (purchaseItemToRemove == null)
            throw new ResourseNotFoundExeption("Purchase item not found");
        var product = await _productRepository.GetByIdAsync(purchaseItemToRemove.ProductId);
        if(product == null)
            throw new ResourseNotFoundExeption("Porduct not found");
        
        product.Stock-=purchaseItemToRemove.Quantity;
        await _productRepository.UpdateAsync(product);
       
       // purchase.PurchaseItems.Remove(purchaseItemToRemove);
        await _purchaseItemRepository.DeleteAsync(purchaseItemId);
        purchase.TotalPrice=purchase.PurchaseItems.Sum(item => item.TotalPrice);
        await  _purchaseRepository.UpdateAsync(purchase);
        return _mapper.Map<PurchaseItemResponse>(purchaseItemToRemove);
    }
 
}