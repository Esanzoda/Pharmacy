using MassTransit;
using MassTransit.Mediator;
using Pharmasy.Messages.Events;
using Pharmasy.Repositories;
using Pharmasy.Services;

namespace Pharmasy.Consumers;

public class OrderCompletedConsumer : IConsumer<OrderCompletedEvent>
{
    private readonly ILogger<OrderCompletedConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMessageService _messageService;

    public OrderCompletedConsumer(
        ILogger<OrderCompletedConsumer> logger,
        IEmailService emailService, IMessageService messageService,
        ICustomerRepository customerRepository)
    {
        _logger = logger;
        _emailService = emailService;
        _messageService = messageService;
        _customerRepository = customerRepository;
    }

    public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Order completed: OrderId={OrderId},  CustomerId={CustomerId}",
            message.OrderId,
            message.CustomerId);

        var user = await _customerRepository.GetByIdAsync(message.CustomerId);
        if (user != null)
        {
            await _emailService.SendOrderCompletedAsync(
                user.Email,
                message.OrderId,
                message.TotalAmount,
                message.CompletedAt);
            await _messageService.SendOrderCompletedAsync(
                user.PhoneNumber,
                message.OrderId,
                message.TotalAmount,
                message.CompletedAt);
        }
    }
}