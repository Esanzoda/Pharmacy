using MassTransit;
using MediatR;
using Pharmasy.Messages.Events;
using Pharmasy.CQRS;
using Pharmasy.CQRS.Notification.Commands;
using Pharmasy.Interfaces;

namespace Pharmasy.Consumers;

public class OrderCanselledConsumer(
    IApplicationDbContext dbContext,
    ILogger<OrderCanselledConsumer> logger,
    IMediator mediator,
    IMessageService messageService
) : IConsumer<OrderCancelledEvent>
{
    public async Task Consume(ConsumeContext<OrderCancelledEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Order cancelled: OrderId={OrderId}, CustomerId={CustomerId}",
            message.OrderId,
            message.CustomerId);


        var user = await dbContext.Customers
            .FindAsync(message.CustomerId);
        if (user != null)
        {
            await mediator.Send(new SendToEmailCustomerOrderCancelledCommand(
                user.Email,
                message.OrderId,
                message.UpdateTime));
                
            await messageService.SendOrderCancelledAsync(
                user.PhoneNumber,
                message.OrderId,
                message.UpdateTime
            );
        }
    }
}