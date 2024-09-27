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
            Host = "mail37.mailasp.net",
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("postmaster@southsidek9camp.com", "iM!58-gXhW?6"),
        };

        MailMessage emailMessage = new()
        {
            From = new MailAddress("postmaster@southsidek9camp.com"),
            To = { new MailAddress(email) },
            IsBodyHtml = true,
            Subject = subject,
            Body = body
        };

        return client.SendMailAsync(emailMessage);
    }
}