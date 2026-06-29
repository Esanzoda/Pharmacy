using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Pharmasy.Models.Domain;

namespace Pharmasy.Services;

public interface IEmailService
{
    Task SendAsync(EmailMessage message);
    Task SendOrderCreatedAsync(string toEmail, long orderId, decimal totalAmount);
    Task SendOrderCancelledAsync(string toEmail, long orderId);
    Task SendOrderCompletedAsync(string toEmail, long orderId, decimal totalAmount);
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
                _configuration["EmailSettings:From"]));

            email.To.Add(MailboxAddress.Parse(message.To));
            email.Subject = message.Subject;

            var builder = new BodyBuilder { HtmlBody = message.Body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _configuration["EmailSettings:Host"],
                _configuration.GetValue<int>("EmailSettings:Port"),
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _configuration["EmailSettings:Username"],
                _configuration["EmailSettings:Password"]);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email sent to {Email}", message.To);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", message.To);
        }
    }

    public async Task SendOrderCreatedAsync(string toEmail, long orderId, decimal totalAmount)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"Заказ #{orderId} создан — Pharmasy",
            Body = $@"
                <h2>Ваш заказ создан!</h2>
                <p>Номер заказа: <strong>#{orderId}</strong></p>
                <p>Сумма: <strong>{totalAmount:C}</strong></p>
                <p>Спасибо за покупку в Pharmasy!</p>
            "
        };
        await SendAsync(message);
    }

    public async Task SendOrderCancelledAsync(string toEmail, long orderId)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"Заказ #{orderId} отменён — Pharmasy",
            Body = $@"
                <h2>Ваш заказ отменён</h2>
                <p>Номер заказа: <strong>#{orderId}</strong></p>
                <p>Если это ошибка — свяжитесь с нами.</p>
            "
        };
        await SendAsync(message);
    }

    public async Task SendOrderCompletedAsync(string toEmail, long orderId, decimal totalAmount)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"Заказ #{orderId} завершён — Pharmasy",
            Body = $@"
                <h2>Ваш заказ завершён!</h2>
                <p>Номер заказа: <strong>#{orderId}</strong></p>
                <p>Итоговая сумма: <strong>{totalAmount:C}</strong></p>
                <p>Спасибо что выбрали Pharmasy!</p>
            "
        };
        await SendAsync(message);
    }

    public async Task SendLowStockAlertAsync(string toEmail, string productName, int currentStock)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"Низкий склад: {productName} — Pharmasy",
            Body = $@"
                <h2>⚠️ Внимание: низкий склад</h2>
                <p>Продукт: <strong>{productName}</strong></p>
                <p>Остаток: <strong>{currentStock} шт.</strong></p>
                <p>Пожалуйста пополните склад.</p>
            "
        };
        await SendAsync(message);
    }
}