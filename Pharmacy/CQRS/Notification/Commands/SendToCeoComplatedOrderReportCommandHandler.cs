using MediatR;
using Pharmasy.Models.Domain;

namespace Pharmasy.CQRS.Notification.Commands;

public record SendToCeoComplatedOrderReportCommand(
    string ToEmail,
    DateTime Day,
    int Count,
    decimal TotlAmount)
    : IRequest;

public class SendToCeoComplatedOrderReportCommandHandler(
    IMediator mediator
) : IRequestHandler<SendToCeoComplatedOrderReportCommand>
{
    public async Task Handle(SendToCeoComplatedOrderReportCommand request, CancellationToken cancellationToken)
    {
        var message = new EmailMessage()
        {
            To = request.ToEmail,
            Subject = $"Report",
            Body = $@"
                <h2>Report Completed order</h2>
                <p>Day:<strong>{request.Day}</strong></p>
                <p>Count:<strong>{request.Count}</strong> </p>
                <p>TotalAmount:<strong>{request.TotlAmount}</strong></p>"
        };
        await mediator.Send(new SendToEmailCommand(message), cancellationToken);
    }
}