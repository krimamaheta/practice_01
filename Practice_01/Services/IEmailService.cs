using System.Net.Mail;

namespace practice1.Services
{
    public interface IEmailService
    {
        bool SendAsync(MailMessage message, string body);
    }
}
