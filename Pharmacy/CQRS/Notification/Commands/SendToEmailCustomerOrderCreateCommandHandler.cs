using MediatR;
using Pharmasy.Models.Domain;

namespace Pharmasy.CQRS.Notification.Commands;

public record SendToEmailCustomerOrderCreateCommand(
    string ToEmail,
    long OrderId,
    decimal TotalAmount,
    DateTime CreatedAt) : IRequest;

public class SendToEmailCustomerOrderCreateCommandHandler(IMediator mediator)
    : IRequestHandler<SendToEmailCustomerOrderCreateCommand>
{
    public async Task Handle(SendToEmailCustomerOrderCreateCommand request, CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = request.ToEmail,
            Subject = $"Order #{request.OrderId} created — Pharmasy",
            Body = $@"
                <h2>Yor order created!</h2>
                <p>order Id: <strong>#{request.OrderId}</strong></p>
                <p>Total amout <strong>{request.TotalAmount:C}</strong></p>
        <p>Order created at <strong>{request.CreatedAt}</strong></p>
                <p>Thents for  order in our Pharmacy</p>

            "
        };
        await mediator.Send(new SendToEmailCommand(message));
    }
}