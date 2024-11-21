
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Restaurants.Domain.HelperModels;

using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configurations;


namespace Restaurants.Infrastructure.Services
{
    public class EmailService : IEmailService
    {

        private readonly EmailConfiguration _emailConfig;
        public EmailService(IOptions<EmailConfiguration> emailConfig) => _emailConfig = emailConfig.Value;
        public void SendEmail(Message message, string email)
        {
            var emailMessage = CreateEmailMessage(message, email);
            Send(emailMessage);
        }



        private MimeMessage CreateEmailMessage(Message message, string email)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.Name, _emailConfig.From));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.CheckCertificateRevocation = false;
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                client.Send(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception or both.
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }

}
