using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Purchase;

public class PurchaseDiBase
{
   protected IPurchaseRepository PurchaseRepository;
   protected readonly IPurchaseItemRepository PurchaseItemRepository;
   protected readonly IEmployeeService EmployeeService;
   protected readonly IProductRepository ProductRepository;
   protected readonly IDistributedCache Cache;
   protected readonly IMapper Mapper;

   public PurchaseDiBase(IPurchaseRepository purchaseRepository)
   {
      PurchaseRepository = purchaseRepository;
   }
   public PurchaseDiBase(IPurchaseRepository purchaseRepository, IPurchaseItemRepository purchaseItemRepository, IEmployeeService employeeService, IProductRepository productRepository, IDistributedCache cache, IMapper mapper)
   {
      PurchaseRepository = purchaseRepository;
      PurchaseItemRepository = purchaseItemRepository;
      EmployeeService = employeeService;
      ProductRepository = productRepository;
      Cache = cache;
      Mapper = mapper;
   }
   
}