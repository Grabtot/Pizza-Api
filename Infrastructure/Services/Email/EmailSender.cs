using Microsoft.AspNetCore.Identity.UI.Services;
using PizzaApi.Application.Common.Interfaces;

namespace PizzaApi.Infrastructure.Services.Email
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
