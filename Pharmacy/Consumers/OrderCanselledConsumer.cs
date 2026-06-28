using MassTransit;
using Pharmasy.Messages.Evants;

namespace Pharmasy.Consumers;

public class OrderCanselledConsumer:IConsumer<OrderCancelledEvant> 
{
    private readonly ILogger<OrderCanselledConsumer> _logger;

    public OrderCanselledConsumer(ILogger<OrderCanselledConsumer> logger)
    {
     _logger = logger;    
    }
    public async Task Consume(ConsumeContext<OrderCancelledEvant> context)
    {
        
        var message = context.Message;
        _logger.LogInformation(
            "OrderCanselled {OrderId} customer{CustomerId} has been cancelled} at {UpdateTime}. ", 
            message.OrderId, 
            message.CustomerId, 
            message.UpdateTime
            );
        Console.WriteLine($"Order {message.OrderId} has been cancelled.");
        
        
        //who need thos sms
    }
}