using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Threading.Tasks;

namespace ScrumPoker.CrossCutting.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfig _emailConfig;
        public EmailSender(IConfiguration config)
        {
            _emailConfig = new EmailConfig()
            {
                From = config.GetSection("EmailConfig:From").Value,
                SmtpServer = config.GetSection("EmailConfig:SmtpServer").Value,
                Port =  int.Parse(config.GetSection("EmailConfig:Port").Value),
                UserName = config.GetSection("EmailConfig:UserName").Value,
                Password = config.GetSection("EmailConfig:Password").Value
            };
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            MimeMessage mineMessage = CreateEmailMessage(email, subject, message);

            using (var client = new SmtpClient())
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                await client.SendAsync(mineMessage);
                client.Disconnect(true);
            }
        }

        private MimeMessage CreateEmailMessage(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("ScrumPoker", _emailConfig.From));
            emailMessage.To.Add(new MailboxAddress(email, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            return emailMessage;
        }
    }
}
