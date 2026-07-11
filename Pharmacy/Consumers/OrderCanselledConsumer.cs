using MassTransit;
using Pharmasy.Messages.Evants;
using Pharmasy.Repositories;
using Pharmasy.Services;

namespace Pharmasy.Consumers;

public class OrderCanselledConsumer : IConsumer<OrderCancelledEvent>
{
    private readonly ILogger<OrderCanselledConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMessageService _messageService;

    public OrderCanselledConsumer(
        ILogger<OrderCanselledConsumer> logger,
        IEmailService emailService,
        ICustomerRepository customerRepository, IMessageService messageService)
    {
        _logger = logger;
        _emailService = emailService;
        _customerRepository = customerRepository;
        _messageService = messageService;
    }

    public async Task Consume(ConsumeContext<OrderCancelledEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Order cancelled: OrderId={OrderId}, CustomerId={CustomerId}",
            message.OrderId,
            message.CustomerId);


        var user = await _customerRepository.GetByIdAsync(message.CustomerId);
        if (user != null)
        {
            await _emailService.SendOrderCancelledAsync(
                user.Email,
                message.OrderId,
                message.UpdateTime);
            await _messageService.SendOrderCancelledAsync(
                user.PhoneNumber,
                message.OrderId,
                message.UpdateTime
            );
        }
    }
}