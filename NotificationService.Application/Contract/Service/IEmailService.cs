using NotificationService.Domain.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Contract.Service
{
    public  interface IEmailService
    {
        Task SendEmail(EmailRequest request);
    }
}
