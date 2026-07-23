using MailKit.Net.Smtp;
using MailKit.Security;
using MediatR;
using MimeKit;
using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Notification.Commands;

public record SendToEmailCommand(
    EmailMessage Message) : IRequest;

public class SendToEmailCommandHandler(
    IConfiguration configuration,
    ILogger<SendToEmailCommandHandler> logger) : IRequestHandler<SendToEmailCommand>
{
    public async Task Handle(SendToEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                configuration["EmailSettings:FromName"],
                configuration["EmailSettings:From"] ?? ""));

            email.To.Add(MailboxAddress.Parse(request.Message.To));
            email.Subject = request.Message.Subject;

            var builder = new BodyBuilder { HtmlBody = request.Message.Body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                configuration["EmailSettings:Host"] ?? throw new InvalidOperationException(),
                configuration.GetValue<int>("EmailSettings:Port"),
                SecureSocketOptions.StartTls, cancellationToken);

            await smtp.AuthenticateAsync(
                configuration["EmailSettings:Username"] ?? throw new InvalidOperationException(),
                configuration["EmailSettings:Password"] ?? throw new InvalidOperationException(),
                cancellationToken);

            await smtp.SendAsync(email,
                cancellationToken);
            await smtp.DisconnectAsync(true,
                cancellationToken);

            logger.LogInformation("Email sent to {Email}", request.Message.To);
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {Email}", request.Message.To);
        }
    }
}