using MediatR;
using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Notification.Commands;

public record SendToCeoCompletedOrderReportCommand(
    string ToEmail,
    DateTime Day,
    int Count,
    decimal TotalAmount)
    : IRequest;

public class SendToCeoCompletedOrderReportCommandHandler(
    IMediator mediator
) : IRequestHandler<SendToCeoCompletedOrderReportCommand>
{
    public async Task Handle(SendToCeoCompletedOrderReportCommand request, CancellationToken cancellationToken)
    {
        var message = new EmailMessage()
        {
            To = request.ToEmail,
            Subject = $"Report",
            Body = $@"
                <h2>Report Completed order</h2>
                <p>Day:<strong>{request.Day}</strong></p>
                <p>Count:<strong>{request.Count}</strong> </p>
                <p>TotalAmount:<strong>{request.TotalAmount}</strong></p>"
        };
        await mediator.Send(new SendToEmailCommand(message), cancellationToken);
    }
}