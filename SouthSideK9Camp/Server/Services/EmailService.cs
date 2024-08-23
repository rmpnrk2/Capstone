using System.Net.Mail;
using System.Net;

namespace SouthSideK9Camp.Server.Services;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string body);
}

public class EmailService : IEmailService
{
    public Task SendEmailAsync(string email, string subject, string body)
    {
        SmtpClient client = new SmtpClient()
        {
            Port = 587,
            Host = "smtp-mail.outlook.com",
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("southsidek9camp@outlook.com", "southsideDB4649T20O6RS"),
        };

        MailMessage emailMessage = new()
        {
            From = new MailAddress("southsidek9camp@outlook.com"),
            To = { new MailAddress(email) },
            Subject = subject,
            IsBodyHtml = true,
            Body = body
        };

        return client.SendMailAsync(emailMessage);
    }
}