using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product?> GetProductByBarcodeAsync(string barcode);
    Task<List<Product>> GetProductsByNameAsync(string name,int page, int pageSize);
    Task<List<Product>> GetProductsByCategoryIdAsync(long categoryId, int page, int pageSize);
    Task<List<Product>> GetOutOfStockAsync(int page, int pageSize);
    Task<List<Product>> GetLowOfStockAsync(int minquantity, int page, int pageSize);
    Task<List<Product>> GetExpiryDateAsync();
    Task<List<Product>> GetProductsByPurchasePriceAsync(decimal price, int page, int pageSize);
    Task<List<Product>> GetProductsByOrderPriceAsync(decimal price, int page, int pageSize);
    Task<List<Product>> GetProductsByCountryAsync(CountryEnum country, int page, int pageSize);
    Task<Product?> GetProductByNameAsync(string name);
    Task<bool> ProductExistsAsync(string name);
}

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
   

    public async Task<Product?> GetProductByBarcodeAsync(string barcode)
    {
        return await DbContext.Products
            .FirstOrDefaultAsync(x => x.Barcode == barcode);
    }

    public async Task<List<Product>> GetProductsByNameAsync(string name, int page, int pageSize)
    {
        return await DbContext.Products
            .Where(x => x.Name.ToLower() == name.ToLower())
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    

//chek for create product
    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return await DbContext.Products
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
    }

    public async Task<List<Product>> GetProductsByCategoryIdAsync(long categoryId, int page, int pageSize)
    {
        return await DbContext.Products
            .Where(x => x.CategoryId == categoryId)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Product>> GetOutOfStockAsync(int page, int pageSize)
    {
        return await DbContext.Products
            .Where(x => x.Stock == 0)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Product>> GetLowOfStockAsync(int minquantity, int page, int pageSize)
    {
        return await DbContext.Products
            .Where(x => x.Stock <= minquantity)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

//its for back job 
    public async Task<List<Product>> GetExpiryDateAsync()
    {
        return await DbContext.Products
            .Where(x => x.ExpiryDate == DateTime.Today && x.Stock > 0)
            .ToListAsync();
    }

    public Task<List<Product>> GetProductsByPurchasePriceAsync(decimal price, int page, int pageSize)
    {
        return DbContext.Products
            .Where(x => x.PurchasePrice == price)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<List<Product>> GetProductsByOrderPriceAsync(decimal price, int page, int pageSize)
    {
        return DbContext.Products
            .Where(x => x.Price == price)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<List<Product>> GetProductsByCountryAsync(CountryEnum country, int page, int pageSize)
    {
        return DbContext.Products
            .Where(x => x.Country == country)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<bool> ProductExistsAsync(string name)
    {
        return DbContext.Products
            .AnyAsync(x => x.Name.ToLower() == name.ToLower());
    }
}