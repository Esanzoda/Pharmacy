using MediatR;
using Pharmasy.Models.Domain;

namespace Pharmasy.CQRS.Notification.Commands;

public record SendToEmailCustomerOrderCompletedCommand(
    string ToEmail,
    long OrderId,
    decimal TotalAmount,
    DateTime CompletedAt) : IRequest;
public class SendToEmailCustomerOrderCompletedCommandHandler(
    IMediator mediator
    ):IRequestHandler<SendToEmailCustomerOrderCompletedCommand>
{
    public async Task Handle(SendToEmailCustomerOrderCompletedCommand request, CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = request.ToEmail,
            Subject = $"Order #{request.OrderId} is completed — Pharmacy",
            Body = $@"
                <h2>Your order is completed at<strong>{request.CompletedAt}</strong></h2>
                <p>Order id: <strong>#{request.OrderId}</strong></p>
                <p>Total amout: <strong>{request.TotalAmount:C}</strong></p>
                <p>Thenkc for choose our  Pharmacy!</p>
            "
        };
       await  mediator.Send(new SendToEmailCommand(message));
    }
}