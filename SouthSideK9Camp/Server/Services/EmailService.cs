using System.Net.Mail;
using System.Net;
using SouthSideK9Camp.Server.JSONSettings;
using Microsoft.Extensions.Options;

namespace SouthSideK9Camp.Server.Services;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public Task SendEmailAsync(string email, string subject, string body)
    {
        SmtpClient client = new SmtpClient()
        {
            Port = 587,
            Host = "mail73.mailasp.net",
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password),
        };

        MailMessage emailMessage = new()
        {
            From = new MailAddress(_emailSettings.Email),
            To = { new MailAddress(email) },
            IsBodyHtml = true,
            Subject = subject,
            Body = body
        };

        // emailMessage.Headers.Add("Message-Id", $"<{Guid.NewGuid()}@southsidek9camp.com>");

        return client.SendMailAsync(emailMessage);
    }
}