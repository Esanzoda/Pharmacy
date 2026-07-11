using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product;

public class ProductDiBase
{
    protected readonly IProductRepository ProductRepository;
    protected readonly ICategoryRepository CategoryRepository;
    protected readonly IMapper Mapper;
    protected readonly IDistributedCache Cache;

    public ProductDiBase(IProductRepository productRepository)
    {
        ProductRepository = productRepository;
    }
    public ProductDiBase(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper, IDistributedCache cache)
    {
        ProductRepository = productRepository;
        CategoryRepository = categoryRepository;
        Mapper = mapper;
        Cache = cache;
    }
    
}