
using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
namespace Pharmasy.Jobs;

public class ChekExpiraDateProduct
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ChekExpiraDateProduct> _logger;
    public ChekExpiraDateProduct(AppDbContext dbContext, ILogger<ChekExpiraDateProduct> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
       
    }

    public async Task ChekExpiraDateAsync(int hoursOld = 24)
    {
        _logger.LogInformation(
            $"Starting chek expira date product");
        var nextTime = DateTime.UtcNow.AddHours(hoursOld);
        var expiridateProduct =await _dbContext.Products
            .Where(x => x.ExpiryDate == DateTime.UtcNow &&
                        x.Stock > 0
                        && x.CreateAt > nextTime)
            .ToListAsync();
        if (!expiridateProduct.Any())
        {
            _logger.LogInformation(
                $"No expire date products found");
            throw new ResourseNotFoundExeption("No products found");
        }

        var expire = new ExpireDateProduct
        {
            CreateAt = DateTime.Now,
            TotalOrderPrice = 0,
            TotalPurchasePrice = 0,
            
        };
        _dbContext.ExpireDateProducts.Add(expire);

        foreach (var product in expiridateProduct)
        {
            var expireDate = new ExpireDateItems
            {
                CreateAt =  DateTime.UtcNow,
                Quantity = product.Stock,
                TotalOrderPrice = product.Stock * product.Price,
                TotalPurchasePrice = product.Stock * product.PurchasePrice
            };
            _dbContext.ExpireDateItems.Add(expireDate);
            expire.TotalOrderPrice+=expire.TotalOrderPrice;
            expire.TotalPurchasePrice+=expireDate.TotalPurchasePrice;
             _dbContext.ExpireDateProducts.Update(expire);
            product.Stock = 0;
            product.UpdateAt=DateTime.UtcNow;
            _dbContext.Products.Update(product);
           
        }
    }
}