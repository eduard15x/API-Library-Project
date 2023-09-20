using ITPLibrary.Api.Core.Dtos.Email;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace ITPLibrary.Api.Core.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail(EmailDto emailDto)
        {
            var smtpClient = new SmtpClient(_configuration.GetSection("Mailbox:Host").Value)
            {
                Port = int.Parse(_configuration.GetSection("Mailbox:Port").Value!),
                Credentials = new NetworkCredential(_configuration.GetSection("Mailbox:Email").Value, _configuration.GetSection("Mailbox:AppPassword").Value),
                EnableSsl = true, // Use SSL
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration.GetSection("Mailbox:Email").Value!),
                Subject = emailDto.Subject,
                Body = emailDto.Body,
                IsBodyHtml = true, // Set to true if the body contains HTML
            };

            mailMessage.To.Add(emailDto.ToEmail);

            smtpClient.Send(mailMessage);
        }
    }
}
