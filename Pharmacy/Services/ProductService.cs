using AutoMapper;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface IProductService : IBaseService<ProductRequest, ProductResponse>
{
    Task<ProductResponse> GetProductByBarcodeAsync(string barcode);
    Task<List<ProductResponse>> GetProductsByNameAsync(string name);
    Task<List<ProductResponse>> GetProductsByCategoryIdAsync(long categoryId, int page, int pageSize);
    Task<List<ProductResponse>> GetOutOfStockAsync(int page, int pageSize);
    Task<List<ProductResponse>> GetLowOfStockAsync(int minquantity, int page, int pageSize);
    Task<List<ProductResponse>> GetExpireDateAsync(int page, int pageSize);
    Task<List<ProductResponse>> GetProductsByPurchasePriceAsync(decimal price, int page, int pageSize);
    Task<List<ProductResponse>> GetProductsByOrderPriseAsync(decimal price, int page, int pageSize);
    public Task<List<ProductResponse>> GetProductsByCountryAsync(CountryEnum country, int page, int pageSize);
    Task<bool> ProductExistsAsync(string name);
}

public class  ProductService : BaseService<Product, ProductRequest, ProductResponse>, IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        : base(productRepository, mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public override async Task<ProductResponse> CreateAsync(ProductRequest request)
    {
        var exist = await _productRepository.ProductExistsAsync(request.Name);
        if (exist)
            throw new ResourseIsAlredyExsistExeption("Product already exists");
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category==null)
            throw new ResourseNotFoundExeption("Category not found");
        
        var product = Mapper.Map<Product>(request);
        product.CreateAt = DateTime.UtcNow;
        await _productRepository.CreateAsync(product);
        return Mapper.Map<ProductResponse>(product);
    }

    public override async Task<ProductResponse> UpdateAsync(long id, ProductRequest request)
    {
        var existingEntity = await _productRepository.GetByIdAsync(id);
        if (existingEntity == null)
            throw new ResourseNotFoundExeption("Product not found");

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
            throw new ResourseNotFoundExeption("Category not found");

        var updateProduct=Mapper.Map(request, existingEntity);
        var updateEntity = await _productRepository.UpdateAsync(updateProduct);
        return Mapper.Map<ProductResponse>(updateEntity);
    }


    public async Task<ProductResponse> GetProductByBarcodeAsync(string barcode)
    {
        var product = await _productRepository.GetProductByBarcodeAsync(barcode);
        if (product == null)
            throw new ResourseNotFoundExeption("Product not found");

        return Mapper.Map<ProductResponse>(product);
    }

    public async Task<List<ProductResponse>> GetProductsByNameAsync(string name)
    {
        var product = await _productRepository.GetProductsByNameAsync(name);
        if (!product.Any())
            throw new ResourseNotFoundExeption("Product not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<List<ProductResponse>> GetProductsByCategoryIdAsync(long categoryId, int page, int pageSize)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
            throw new ResourseNotFoundExeption("Category  not found");

        var product = await _productRepository.GetProductsByCategoryIdAsync(categoryId, page, pageSize);
        if (!product.Any())
            throw new ResourseNotFoundExeption("We dont have product  with ");
        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<List<ProductResponse>> GetOutOfStockAsync(int page, int pageSize)
    {
        var product = await _productRepository.GetOutOfStockAsync(page, pageSize);
        if (!product.Any())
            throw new ResourseNotFoundExeption("Product not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<List<ProductResponse>> GetLowOfStockAsync(int minquantity, int page, int pageSize)
    {
        var product = await _productRepository.GetLowOfStockAsync(minquantity, page, pageSize);
        if (!product.Any())
            throw new ResourseNotFoundExeption("Product not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<List<ProductResponse>> GetExpireDateAsync(int page, int pageSize)
    {
        var product = await _productRepository.GetExpireDateAsync(page, pageSize);
        if (!product.Any())
            throw new ResourseNotFoundExeption("Product not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<List<ProductResponse>> GetProductsByPurchasePriceAsync(decimal price, int page, int pageSize)
    {
        var product = await _productRepository.GetProductsByPurchasePriceAsync(price, page, pageSize);
        if (!product.Any())
            throw new ResourseNotFoundExeption("Product not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<List<ProductResponse>> GetProductsByOrderPriseAsync(decimal price, int page, int pageSize)
    {
        var product = await _productRepository.GetProductsByOrderPriseAsync(price, page, pageSize);
        if (!product.Any())
            throw new ResourseNotFoundExeption("Product not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<List<ProductResponse>> GetProductsByCountryAsync(CountryEnum country, int page, int pageSize)
    {
       var product =await  _productRepository.GetProductsByCountryAsync(country, page, pageSize);
       if(!product.Any())
           throw new ResourseNotFoundExeption("Product not found");
       return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<bool> ProductExistsAsync(string name)
    {
        return await _productRepository.ProductExistsAsync(name);
    }
}