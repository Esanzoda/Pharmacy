using MassTransit;
using Pharmasy.Messages.Evants;
using Pharmasy.Repositories;
using Pharmasy.Services;

namespace Pharmasy.Consumers;

public class OrderCanselledConsumer : IConsumer<OrderCancelledEvant>
{
    private readonly ILogger<OrderCanselledConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;

    public OrderCanselledConsumer(
        ILogger<OrderCanselledConsumer> logger,
        IEmailService emailService,
        IUserRepository userRepository)
    {
        _logger = logger;
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<OrderCancelledEvant> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Order cancelled: OrderId={OrderId}, CustomerId={CustomerId}",
            message.OrderId,
            message.UserId);

        
        var user = await _userRepository.GetByIdAsync(message.UserId);
        if (user != null)
        {
            await _emailService.SendOrderCancelledAsync(
                user.Email,
                message.OrderId);
        }
    }
}