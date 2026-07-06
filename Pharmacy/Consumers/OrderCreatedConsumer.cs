using MassTransit;
using Pharmasy.Messages.Evants;
using Pharmasy.Repositories;
using Pharmasy.Services;

namespace Pharmasy.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvant>
{
    private readonly ILogger<OrderCreatedConsumer> _logger;
    private readonly IEmailService _emailService;

    private readonly ICustomerRepository _customerRepository;

    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger, IEmailService emailService,
        ICustomerRepository customerRepository)
    {
        _logger = logger;
        _emailService = emailService;
        _customerRepository = customerRepository;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvant> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Order created: OrderId={OrderId}, CustomerId={CustomerId}, Total={Total} , CreatedAt={CreatededAt}",
            message.OrderId,
            message.UserId,
            message.TotalAmout,
            message.CreatedAt);

        var user = await _customerRepository.GetByIdAsync(message.UserId);
        if (user != null)
        {
            await _emailService.SendOrderCreatedAsync(
                user.Email,
                message.OrderId,
                message.TotalAmout,
                message.CreatedAt);
        }
    }
}