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
    Task<List<ProductResponse>> GetProductsByPurchasePriceAsync(decimal price, int page, int pageSize);
    Task<List<ProductResponse>> GetProductsByOrderPriseAsync(decimal price, int page, int pageSize);
    public Task<List<ProductResponse>> GetProductsByCountryAsync(CountryEnum country, int page, int pageSize);
    Task<bool> ProductExistsAsync(string name);
}

public class ProductService : BaseService<Product, ProductRequest, ProductResponse>, IProductService
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
        var productByName = await _productRepository.ProductExistsAsync(request.Name);
        if (productByName)
            throw new ResourseIsAlredyExsistExeption($"Product already exists whith this name {request.Name}");
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
            throw new ResourseNotFoundExeption($"Category with this[{request.CategoryId}] not found");
        var productByBarcode = await _productRepository.GetProductByBarcodeAsync(request.Barcode);
        if (productByBarcode != null)
        {
            throw new ResourseIsAlredyExsistExeption($"Product already exists with Barcode {request.Barcode}");
        }

        var product = Mapper.Map<Product>(request);
        await _productRepository.CreateAsync(product);
        await _productRepository.SavechangesAsync();
        return Mapper.Map<ProductResponse>(product);
    }

    public override async Task<ProductResponse> UpdateAsync(long id, ProductRequest request)
    {
        var existingEntity = await _productRepository.GetByIdAsync(id);
        if (existingEntity == null)
            throw new ResourseNotFoundExeption($"Product whith this id {id} not found");

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
            throw new ResourseNotFoundExeption($"Category whis this id {id} not found");
        var productByName = await _productRepository.GetProductByNameAsync(request.Name);
        if (productByName != null)
        {
            throw new ResourseIsAlredyExsistExeption($"Product already exists with Name {request.Name}");
        }

        var productByBarcode = await _productRepository.GetProductByBarcodeAsync(request.Barcode);
        if (productByBarcode != null)
        {
            throw new ResourseIsAlredyExsistExeption($"Product already exists with Barcode {request.Barcode}");
        }


        var updateProduct = Mapper.Map(request, existingEntity);
        var updateEntity = await _productRepository.UpdateAsync(updateProduct);
        await _productRepository.SavechangesAsync();
        return Mapper.Map<ProductResponse>(updateEntity);
    }


    public async Task<ProductResponse> GetProductByBarcodeAsync(string barcode)
    {
        var product = await _productRepository.GetProductByBarcodeAsync(barcode);
        if (product == null)
            throw new ResourseNotFoundExeption("Product whith this barcode not found");

        return Mapper.Map<ProductResponse>(product);
    }

    public async Task<List<ProductResponse>> GetProductsByNameAsync(string name)
    {
        var product = await _productRepository.GetProductsByNameAsync(name);
        if (!product.Any())
            throw new ResourseNotFoundExeption("Product with this naame not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<List<ProductResponse>> GetProductsByCategoryIdAsync(long categoryId, int page, int pageSize)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
            throw new ResourseNotFoundExeption("Category with this id  not found");

        var product = await _productRepository.GetProductsByCategoryIdAsync(categoryId, page, pageSize);
        if (!product.Any())
            throw new ResourseNotFoundExeption("We dont have product  with categoryId ");
        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<List<ProductResponse>> GetOutOfStockAsync(int page, int pageSize)
    {
        var product = await _productRepository.GetOutOfStockAsync(page, pageSize);
        //epty list or eror meet with others
        if (!product.Any())
            throw new ResourseNotFoundExeption("Product  not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<List<ProductResponse>> GetLowOfStockAsync(int minquantity, int page, int pageSize)
    {
        var product = await _productRepository.GetLowOfStockAsync(minquantity, page, pageSize);
        if (!product.Any())
            throw new ResourseNotFoundExeption("Product not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }


    public async Task<List<ProductResponse>> GetProductsByPurchasePriceAsync(decimal price, int page, int pageSize)
    {
        var product = await _productRepository.GetProductsByPurchasePriceAsync(price, page, pageSize);
        if (!product.Any())
            throw new ResourseNotFoundExeption("Product with this purchase price  not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<List<ProductResponse>> GetProductsByOrderPriseAsync(decimal price, int page, int pageSize)
    {
        var product = await _productRepository.GetProductsByOrderPriseAsync(price, page, pageSize);
        if (!product.Any())
            throw new ResourseNotFoundExeption("Product with this price  not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<List<ProductResponse>> GetProductsByCountryAsync(CountryEnum country, int page, int pageSize)
    {
        var product = await _productRepository.GetProductsByCountryAsync(country, page, pageSize);
        if (!product.Any())
            throw new ResourseNotFoundExeption($"Product from this country[{country}] not found");
        return Mapper.Map<List<ProductResponse>>(product);
    }

    public async Task<bool> ProductExistsAsync(string name)
    {
        return await _productRepository.ProductExistsAsync(name);
    }
}