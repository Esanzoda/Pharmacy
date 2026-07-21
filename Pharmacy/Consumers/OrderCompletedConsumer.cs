using MassTransit;
using MediatR;
using Pharmasy.Messages.Events;
using Pharmasy.CQRS;
using Pharmasy.CQRS.Notification.Commands;
using Pharmasy.Interfaces;

namespace Pharmasy.Consumers;

public class OrderCompletedConsumer(
    ILogger<OrderCompletedConsumer> logger,
    IMediator mediator,
    IMessageService messageService,
    IApplicationDbContext dbContext
    ) : IConsumer<OrderCompletedEvent>
{
    public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Order completed: OrderId={OrderId},  CustomerId={CustomerId}",
            message.OrderId,
            message.CustomerId);

        var user = await dbContext.Customers
            .FindAsync(message.CustomerId);
        if (user != null)
        {
            await mediator.Send(new SendToEmailCustomerOrderCompletedCommand(
                user.Email,
                message.OrderId,
                message.TotalAmount,
                message.CompletedAt));
            await messageService.SendOrderCompletedAsync(
                user.PhoneNumber,
                message.OrderId,
                message.TotalAmount,
                message.CompletedAt);
        }
    }
}