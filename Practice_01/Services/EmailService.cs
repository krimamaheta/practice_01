using System.Net;
using System.Net.Mail;

namespace practice1.Services
{
    public class EmailService : IEmailService
    {
        public bool SendAsync(MailMessage message, string body)
        {
            SmtpClient client = new SmtpClient("smtp.ethereal.email", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("estella.macejkovic@ethereal.email", "aTskfnv8TQjPgxxSnz");

            message.From = new MailAddress("estella.macejkovic@ethereal.email");


            // Set the message body
            message.Body = body;
            message.IsBodyHtml = true;
           
            // Send email
            client.Send(message);
            return true;
        }
    }
}
