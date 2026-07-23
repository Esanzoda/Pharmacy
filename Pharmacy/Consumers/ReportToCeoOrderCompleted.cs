using MassTransit;
using MediatR;
using Pharmacy.CQRS.Notification.Commands;
using Pharmacy.Messages.Events;

namespace Pharmacy.Consumers;

public class ReportToCeoOrderCompleted(
    IMediator mediator,
    ILogger<ReportToCeoOrderCompleted> logger) : IConsumer<OrderCompletedEventReportToCeo>
{
    public async Task Consume(ConsumeContext<OrderCompletedEventReportToCeo> context)
    {
        var message = context.Message;
        logger.LogInformation(
            "At {Day} our pharmacy had {Count} completed orders with total amount {TotalAmount}.",
            message.Day,
            message.Count,
            message.TotalAmount);

        if (message.To != null)
        {
            await mediator.Send(new SendToCeoCompletedOrderReportCommand(
                message.To,
                message.Day,
                message.Count,
                message.TotalAmount));
        }
    }
}