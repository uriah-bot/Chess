using Chess.Model;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;

namespace Chess.Service
{
    public interface IEmailService
    {
        Task<(bool successful, string exception)> SendEmail(string subject, string body);
    }

    public class EmailService : IEmailService
    {
        public async Task<(bool successful, string exception)> SendEmail(string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(AppConstants.APP_EMAIL, "upbz xvsd rike fspc"),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(AppConstants.APP_EMAIL),
                    Subject = $"[Custom Chess Feedback] {subject}",
                    Body = $"Message from User:\n\n{body}",
                };

                mailMessage.To.Add(AppConstants.APP_EMAIL);
                await smtpClient.SendMailAsync(mailMessage);

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
