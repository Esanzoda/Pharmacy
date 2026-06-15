using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface IProductRepository : IBaseRepository<Product> 
{
   Task<Product>GetProductByBarcodeAsync(string barcode);
   Task<List<Product>> GetProductsByNameAsync(string name);
   Task<List<Product>> GetProductsByCategoryIdAsync(long categoryId,int page , int pageSize );
   Task<List<Product>> GetOutOfStockAsync(int page, int pageSize);
   Task<List<Product>> GetLowOfStockAsync(int minquantity, int page, int pageSize);
   Task<List<Product>> GetExpireDateAsync(int page, int pageSize);
   Task<List<Product>> GetProductsByPurchasePriceAsync(decimal price, int page, int pageSize);
   Task<List<Product>> GetProductsByOrderPriseAsync(decimal price, int page, int pageSize);
   Task<bool> ProductExistsAsync(string name);

   
}
public class ProductRepository:BaseRepository<Product>,IProductRepository
{
    public ProductRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Product?> GetProductByBarcodeAsync(string barcode)
    {
        return await _dbContext.Products
            .FindAsync(barcode);
    }

    public async Task<List<Product>> GetProductsByNameAsync(string name)
    {
        return await _dbContext.Products
            .Where(x => x.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();
            
            

    }

    public async Task<List<Product>> GetProductsByCategoryIdAsync(long categoryId,int page, int pageSize)
    {
        return await _dbContext.Products
            .Where(x => x.CategoryId == categoryId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Product>> GetOutOfStockAsync(int page, int pageSize)
    {
        return await _dbContext.Products
            .Where(x => x.Stock == 0)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync  ();
    }

    public async Task<List<Product>> GetLowOfStockAsync(int minquantity,int page, int pageSize)
    {
        return await  _dbContext.Products
            .Where(x=>x.Stock<=minquantity)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Product>> GetExpireDateAsync(int page, int pageSize)
    {
      return await  _dbContext.Products
          .Where(x=>x.ExpiryDate==DateTime.Today)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();
    }

    public Task<List<Product>> GetProductsByPurchasePriceAsync(decimal price, int page, int pageSize)
    {
        return _dbContext.Products
            .Where(x=>x.PurchasePrice==price)
            .Skip((page-1)*pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<List<Product>> GetProductsByOrderPriseAsync(decimal price, int page, int pageSize)
    {
        return _dbContext.Products
            .Where(x=>x.OrderPrice==price)
            .Skip((page-1)*pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<bool> ProductExistsAsync(string name)
    {
        return _dbContext.Products
                .AnyAsync(x => x.Name.ToLower()==name.ToLower());
        
    }
}