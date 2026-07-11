using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Messages.Evants;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Repositories;

//to do : use job for send ceo ochot 
namespace Pharmasy.Jobs;

public class ChekExpiraDateProduct
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ChekExpiraDateProduct> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IOrderRepository _orderRepository;

    public ChekExpiraDateProduct(AppDbContext dbContext, ILogger<ChekExpiraDateProduct> logger,
        IPublishEndpoint publishEndpoint, IOrderRepository orderRepository)
    {
        _dbContext = dbContext;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _orderRepository = orderRepository;
    }


    public async Task ChekExpiraDateAsync(int hoursOld = 24)
    {
        _logger.LogInformation(
            $"Starting chek expira date product");

        var nextTime = DateTime.UtcNow.AddHours(hoursOld);

        var expiridateProduct = await _dbContext.Products
            .Where(x => x.ExpiryDate <= DateTime.UtcNow && x.Stock > 0)
            .ToListAsync();

        if (!expiridateProduct.Any())
        {
            _logger.LogInformation($"No expire date products found");
            return;
        }

        var expireRecord = new ExpiryDateProduct
        {
            TotalOrderPrice = 0,
            TotalPurchasePrice = 0,
        };

        _dbContext.ExpireDateProducts.Add(expireRecord);

        foreach (var product in expiridateProduct)
        {
            var expireItem = new ExpireDateItems
            {
                ProductId = product.Id,
                Quantity = product.Stock,
                ProductName = product.Name,
                TotalOrderPrice = product.Stock * product.Price,
                TotalPurchasePrice = product.Stock * product.PurchasePrice
            };

            _dbContext.ExpireDateItems.Add(expireItem);

            expireRecord.TotalOrderPrice += expireItem.TotalOrderPrice;
            expireRecord.TotalPurchasePrice += expireItem.TotalPurchasePrice;

            var oldStock = product.Stock;

            product.Stock = 0;

            _logger.LogInformation($"Finished chek expira date product");
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task ReportToCeo(int hoursOld = 24)
    {
        _logger.LogInformation(
            $"Starting report to ceo");
        var yesterday = DateTime.UtcNow.AddDays(-1);
        var nextTime = DateTime.UtcNow.AddHours(hoursOld);
        decimal TotalAmount = 0;
        var completedOrder = await _orderRepository.GetOrdersByOrderStatusAndDayAsync(OrderStatus.Completed, yesterday);
        foreach (var order in completedOrder)
        {
            TotalAmount += order.TotalAmount;
        }


        await _publishEndpoint.Publish(new OrderCompletedEventReportToCeo()
        {
            day = yesterday,
            count = completedOrder.Count,
            totalamout = TotalAmount
        });
        var cancelledOrder = await _orderRepository.GetOrdersByOrderStatusAndDayAsync(OrderStatus.Cancelled, yesterday);
        await _publishEndpoint.Publish(new OrderCancelledEvantToCeo()
        {
            DateTime = yesterday,
            Count = cancelledOrder.Count,
        });
        var shipedorder = await _orderRepository.GetOrdersByOrderStatusAndDayAsync(OrderStatus.Shipped, yesterday);

        _logger.LogInformation(
            $"Finished report cheking");
        _logger.LogInformation(
            $"Starting report to ceo");
    }
}