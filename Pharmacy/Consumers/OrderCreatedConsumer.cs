
using MassTransit;
using Pharmasy.Messages.Evants;


namespace Pharmasy.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvant>
{
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
    {
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<OrderCreatedEvant> context)
    {
        var message = context.Message;
        _logger.LogInformation(
            "Order created: OrderId={OrderId}, CustomerId={CustomerId}, Total={Total}",
            message.OrderId,
            message.CustomerId,
            message.TotalAmout);
        Console.WriteLine($"Order {message.OrderId}  created. totalamouy {message.TotalAmout}");
        //await other services which use it message kak consumer 
        await Task.CompletedTask;
    }
}