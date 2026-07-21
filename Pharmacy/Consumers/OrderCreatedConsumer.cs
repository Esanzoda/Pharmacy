using MassTransit;
using MediatR;
using Pharmasy.Messages.Events;
using Pharmasy.CQRS.Notification.Commands;
using Pharmasy.Interfaces;

namespace Pharmasy.Consumers;

public class OrderCreatedConsumer(
    ILogger<OrderCreatedConsumer> logger,
    IMediator mediator,
    IApplicationDbContext dbContext
    ) : IConsumer<OrderCreatedEvent>
{
   

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Order created: OrderId={OrderId}, CustomerId={CustomerId}, Total={Total} , CreatedAt={CreatededAt}",
            message.OrderId,
            message.CustomerId,
            message.TotalAmount,
            message.CreatedAt);

        var user = await dbContext.Customers
            .FindAsync(message.CustomerId);
        if (user != null)
        {
            await  mediator.Send(new SendToEmailCustomerOrderCreateCommand(
                user.Email,
                message.OrderId,
                message.TotalAmount,
                message.CreatedAt));
        }
    }
}