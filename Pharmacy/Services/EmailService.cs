using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Pharmasy.Models.Domain;

namespace Pharmasy.Services;

public interface IEmailService
{
    Task SendAsync(EmailMessage message);
    Task SendOrderCreatedAsync(string toEmail, long orderId, decimal totalAmount, DateTime createdAt);
    Task SendOrderCancelledAsync(string toEmail, long orderId, DateTime updatedAt);
    Task SendOrderCompletedAsync(string toEmail, long orderId, decimal totalAmount, DateTime completedAt);
    Task SendLowStockAlertAsync(string toEmail, string productName, int currentStock);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendAsync(EmailMessage message)
    {
        try
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                _configuration["EmailSettings:FromName"],
                _configuration["EmailSettings:From"] ?? ""));

            email.To.Add(MailboxAddress.Parse(message.To));
            email.Subject = message.Subject;

            var builder = new BodyBuilder { HtmlBody = message.Body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _configuration["EmailSettings:Host"] ?? throw new InvalidOperationException() ,
                _configuration.GetValue<int>("EmailSettings:Port"),
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _configuration["EmailSettings:Username"] ?? throw new InvalidOperationException(),
                _configuration["EmailSettings:Password"] ?? throw new InvalidOperationException());

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email sent to {Email}", message.To);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", message.To);
        }
    }

    public async Task SendOrderCreatedAsync(string toEmail, long orderId, decimal totalAmount, DateTime createdAt)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"Order #{orderId} created — Pharmasy",
            Body = $@"
                <h2>Yor order created!</h2>
                <p>order Id: <strong>#{orderId}</strong></p>
                <p>Total amout <strong>{totalAmount:C}</strong></p>
        <p>Order created at <strong>{createdAt}</strong></p>
                <p>Thents for  order in our Pharmacy</p>

            "
        };
        await SendAsync(message);
    }

    public async Task SendOrderCancelledAsync(string toEmail, long orderId, DateTime updatedAt)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"order #{orderId} cncselled — Pharmacy",
            Body = $@"
                <h2>Your order is canselled</h2>
                <p>Order Id: <strong>#{orderId}</strong></p>
                <p>Order cancelled at <strong>{updatedAt}</strong></p>
                
            "
        };
        await SendAsync(message);
    }

    public async Task SendOrderCompletedAsync(string toEmail, long orderId, decimal totalAmount, DateTime completedAt)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"Order #{orderId} is completed — Pharmacy",
            Body = $@"
                <h2>Your order is completed at<strong>{completedAt}</strong></h2>
                <p>Order id: <strong>#{orderId}</strong></p>
                <p>Total amout: <strong>{totalAmount:C}</strong></p>
                <p>Thenkc for choose our  Pharmacy!</p>
            "
        };
        await SendAsync(message);
    }

    public async Task SendLowStockAlertAsync(string toEmail, string productName, int currentStock)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"Low Stock: {productName} — Pharmacy",
            Body = $@"
                <h2>️ Attention: Low stock</h2>
                <p>Product: <strong>{productName}</strong></p>
                <p>Remaining: <strong>{currentStock} units.</strong></p>
                <p>Please replenish the stock..</p>
            "
        };
        await SendAsync(message);
    }
}