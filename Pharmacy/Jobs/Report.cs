using MassTransit;
using Pharmasy.Data;
using Pharmasy.Messages.Events;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Repositories;

namespace Pharmasy.Jobs;

public class Report
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ChekExpiraDateProduct> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IOrderRepository _orderRepository;

    public Report(AppDbContext dbContext, ILogger<ChekExpiraDateProduct> logger, IPublishEndpoint publishEndpoint, IOrderRepository orderRepository)
    {
        _dbContext = dbContext;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _orderRepository = orderRepository;
    }
    public async Task ReportToCeo(int hoursOld = 24)
    {
        _logger.LogInformation(
            $"Starting report to ceo");
        var yesterday = DateTime.UtcNow.AddDays(-1);
        var nextTime = DateTime.UtcNow.AddHours(hoursOld);
        decimal totalAmount = 0;
        var completedOrder = await _orderRepository.GetOrdersByOrderStatusAndDayAsync(OrderStatus.Completed, yesterday);
        foreach (var order in completedOrder)
        {
            totalAmount += order.TotalAmount;
        }


        await _publishEndpoint.Publish(new OrderCompletedEventReportToCeo()
        {
            day = yesterday,
            count = completedOrder.Count,
            totalamout = totalAmount
        });
        var cancelledOrder = await _orderRepository.GetOrdersByOrderStatusAndDayAsync(OrderStatus.Cancelled, yesterday);
        await _publishEndpoint.Publish(new OrderCancelledEvantToCeo()
        {
            DateTime = yesterday,
            Count = cancelledOrder.Count,
        });
        var shippedOrders = await _orderRepository.GetOrdersByOrderStatusAndDayAsync(OrderStatus.Shipped, yesterday);
        await _publishEndpoint.Publish(new OrderShippedEventToCeo
        {
            Count = shippedOrders.Count,
            DateTime = yesterday
        });
        _logger.LogInformation(
            $"Finished report cheking");
        _logger.LogInformation(
            $"Starting report to ceo");
    }
}