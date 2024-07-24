using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using NotificationService.Application.Configuration;
using NotificationService.Application.Contract.Service;
using NotificationService.Domain.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public void SendEmail(EmailRequest request)
        {
            var emailMessage =  CreateEmailMessage(request);
            Send(emailMessage);
        }


        
        private MimeMessage CreateEmailMessage(EmailRequest request)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_emailConfig.From));
            emailMessage.To.Add(MailboxAddress.Parse(request.To));
            emailMessage.Subject = request.Subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var smtp = new SmtpClient();
            smtp.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
            smtp.Authenticate(_emailConfig.Username, _emailConfig.Password);
            smtp.Send(mailMessage);
            smtp.Disconnect(true);
        }
    }
}
