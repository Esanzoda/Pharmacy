using MassTransit;
using MediatR;
using Pharmasy.CQRS.Notification.Commands;
using Pharmasy.Messages.Events;

namespace Pharmasy.Consumers;

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
            message.Totalamout);

        if (message.To != null)
        {
            await mediator.Send(new SendToCeoComplatedOrderReportCommand(
                message.To,
                message.Day,
                message.Count,
                message.Totalamout));
        }
    }
}