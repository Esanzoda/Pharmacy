using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
using Pharmacy.Jobs;
using Pharmacy.Messages.Events;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.ExpiredProducts.Commands;

public record ReportCommand : IRequest;

public class ReportcommandHandler(
    AppDbContext dbContext,
    ILogger<CheckExpiredProductsJob> logger,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<ReportCommand>
{
    public async Task Handle(ReportCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting check to repo");
        var yesterday = DateTime.UtcNow; //.AddDays(-1);
        decimal totalAmount = 0;
        var completedOrders = await dbContext.Orders
            .Where(x => x.OrderStatus == OrderStatus.Completed)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        foreach (var order in completedOrders)
        {
            totalAmount += order.TotalAmount;
        }

        await publishEndpoint.Publish(new OrderCompletedEventReportToCeo()
        {
            To = "orashesanov05@gmail.com",
            Count = completedOrders.Count,
            Day = DateTime.UtcNow,
            Totalamout = totalAmount
        }, cancellationToken);
        logger.LogInformation("OrderCompletedEventReportToCeo published");
        var cancelleddOrders = await dbContext.Orders
            .Where(x => x.OrderStatus == OrderStatus.Cancelled && x.CreatedAt == yesterday)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        await publishEndpoint.Publish(new OrderCancelledEvantToCeo()
        {
            DateTime = yesterday,
            Count = cancelleddOrders.Count
        }, cancellationToken);
        var sheepedOrders = await dbContext.Orders
            .Where(x => x.OrderStatus == OrderStatus.Shipped && x.CreatedAt == yesterday)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        await publishEndpoint.Publish(new OrderShippedEventToCeo
        {
            Count = sheepedOrders.Count,
            DateTime = yesterday
        }, cancellationToken);
    }
}