using MediatR;
using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Notification.Commands;

public record SendToEmailCustomerOrderCancelledCommand(
    string ToEmail,
    long OrderId,
    DateTime UpdatedAt) : IRequest;

public class SendToEmailCustomerOrderCancelledCommandHandler(
    IMediator mediator) : IRequestHandler<SendToEmailCustomerOrderCancelledCommand>
{
    public async Task Handle(SendToEmailCustomerOrderCancelledCommand request, CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = request.ToEmail,
            Subject = $"order #{request.OrderId} cancelled — Pharmacy",
            Body = $@"
                <h2>Your order is cancelled</h2>
                <p>Order Id: <strong>#{request.OrderId}</strong></p>
                <p>Order cancelled at <strong>{request.UpdatedAt}</strong></p>
                
            "
        };
        await mediator.Send(new SendToEmailCommand(message), cancellationToken);
    }
}