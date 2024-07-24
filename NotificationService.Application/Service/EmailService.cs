using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using NotificationService.Application.Configuration;
using NotificationService.Application.Contract;
using NotificationService.Application.Contract.Service;
using NotificationService.Domain.DTOs.Request;
using NotificationService.Domain.Enums;
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
        private readonly IMongoDbLogRepository _mongodb;

        public EmailService(EmailConfiguration emailConfig, IMongoDbLogRepository mongodb)
        {
            _emailConfig = emailConfig;
            _mongodb = mongodb;
        }
        public async Task SendEmail(EmailRequest request)
        {
            var emailMessage =  CreateEmailMessage(request);
            Send(emailMessage);


            var notificationActivity = new NotificationActivity()
            {
                SentTo = request.To,
                SentAt = DateTime.Now,
                Status = true,
                Purpose = request.Subject,
                NotificationType = NotificationType.Email,
                HasAttachment = true,

            };
            await _mongodb.CreateLog(notificationActivity);
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
