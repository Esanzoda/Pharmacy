using MassTransit;
using Pharmasy.Messages.Evants;
using Pharmasy.Repositories;
using Pharmasy.Services;

namespace Pharmasy.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvant>
{
    private readonly ILogger<OrderCreatedConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;

    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger, IEmailService emailService,
        IUserRepository userRepository)
    {
        _logger = logger;
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvant> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Order created: OrderId={OrderId}, CustomerId={CustomerId}, Total={Total}",
            message.OrderId,
            message.UserId,
            message.TotalAmout);
        
        var user = await _userRepository.GetByIdAsync(message.UserId);
        if (user != null)
        {
            await _emailService.SendOrderCreatedAsync(
                user.Email,
                message.OrderId,
                message.TotalAmout);
        }
    }
}