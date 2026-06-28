
using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Services;

namespace Pharmasy.Jobs;

public class ChekExpiraDateProduct
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ChekExpiraDateProduct> _logger;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    public ChekExpiraDateProduct(AppDbContext dbContext, ILogger<ChekExpiraDateProduct> logger, IEmailService emailService,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _logger = logger;
        _emailService = emailService;
        _configuration = configuration;
       
    }

    public async Task ChekExpiraDateAsync(int hoursOld = 24)
    {
        _logger.LogInformation(
            $"Starting chek expira date product");
        var nextTime = DateTime.UtcNow.AddHours(hoursOld);
        var expiridateProduct =await _dbContext.Products
            .Where(x => x.ExpiryDate <= DateTime.UtcNow &&
                        x.Stock > 0
                     //   && x.CreateAt > nextTime
                     )
            .ToListAsync();
        if (!expiridateProduct.Any())
        {
            _logger.LogInformation(
                $"No expire date products found");
            return;
        }
      

        var expireRecord = new ExpireDateProduct
        { 
            CreateAt = DateTime.Now,
            TotalOrderPrice = 0,
            TotalPurchasePrice = 0,
            
        };
        _dbContext.ExpireDateProducts.Add(expireRecord);

        foreach (var product in expiridateProduct)
        {
            var expireItem = new ExpireDateItems
            {
                CreateAt =  DateTime.UtcNow,
                ProductId =  product.Id,
                Quantity = product.Stock,
                ProductName =  product.Name,
                TotalOrderPrice = product.Stock * product.Price,
                TotalPurchasePrice = product.Stock * product.PurchasePrice
            };
            _dbContext.ExpireDateItems.Add(expireItem);
            expireRecord.TotalOrderPrice+=expireItem.TotalOrderPrice;
            expireRecord.TotalPurchasePrice+=expireItem.TotalPurchasePrice;
                            // _dbContext.ExpireDateProducts.Update(expire);
            var oldStock = product.Stock;
            product.Stock = 0;
            product.UpdateAt=DateTime.UtcNow;
            _dbContext.Products.Update(product);

            if (oldStock <= 10)
            {
                var adminEmail = _configuration["EmailSettings:From"];
                await _emailService.SendLowStockAlertAsync(
                    adminEmail,
                    product.Name,
                    product.Stock);
            }
            _dbContext.ExpireDateProducts.Update(expireRecord);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation(
                $"Finished chek expira date product");

        }
    }
}