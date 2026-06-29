using MassTransit;
using Pharmasy.Messages.Evants;
using Pharmasy.Repositories;
using Pharmasy.Services;

namespace Pharmasy.Consumers;

public class OrderCompletedConsumer : IConsumer<OrderCompletedEvant>
{
    private readonly ILogger<OrderCompletedConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;

    public OrderCompletedConsumer(
        ILogger<OrderCompletedConsumer> logger,
        IEmailService emailService,
        IUserRepository userRepository)
    {
        _logger = logger;
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<OrderCompletedEvant> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Order completed: OrderId={OrderId},  CustomerId={CustomerId}",
            message.OrderId,
            message.UserId);

        var user = await _userRepository.GetByIdAsync(message.UserId);
        if (user != null)
        {
            await _emailService.SendOrderCompletedAsync(
                user.Email,
                message.OrderId,
                message.TotalAmout);
        }
    }
}