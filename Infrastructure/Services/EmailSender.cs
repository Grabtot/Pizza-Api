using Microsoft.AspNetCore.Identity.UI.Services;
using PizzaApi.Infrastructure.Common.Interfaces;

namespace PizzaApi.Infrastructure.Services
{
    public class EmailSender(IEmailService emailService) : IEmailSender
    {
        private readonly IEmailService _emailService = emailService;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await _emailService.Send(email, subject, htmlMessage);
        }
    }
}
