using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Contract.Service;
using NotificationService.Domain.DTOs.Request;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public NotificationController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotiication(EmailRequest request)
        {
            await  _emailService.SendEmail(request);
            return Ok();
        }
    }
}
