using MediatR;
using Pharmasy.Models.Domain;

namespace Pharmasy.CQRS.Notification.Commands;

public record SendToEmailLowStockProductCommand(
    string ToEmail,
    string ProductName,
    int CurrentStock
) : IRequest;

public class SendToEmailLowStockProductCommandHandler(
    IMediator mediator
) : IRequestHandler<SendToEmailLowStockProductCommand>
{
    public async Task Handle(SendToEmailLowStockProductCommand request, CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = request.ToEmail,
            Subject = $"Low Stock: {request.ProductName} — Pharmacy",
            Body = $@"
                <h2>️ Attention: Low stock</h2>
                <p>Product: <strong>{request.ProductName}</strong></p>
                <p>Remaining: <strong>{request.CurrentStock} units.</strong></p>
                <p>Please replenish the stock..</p>
            "
        };
        await mediator.Send(new SendToEmailCommand(message));
    }
}