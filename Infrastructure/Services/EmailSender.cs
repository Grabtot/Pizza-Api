using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using PizzaApi.Domain.Users;
using PizzaApi.Infrastructure.Common.Interfaces;

namespace PizzaApi.Infrastructure.Services
{
    public class EmailSender(IEmailService emailService) : IEmailSender, IEmailSender<User>
    {
        private readonly IEmailService _emailService = emailService;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await _emailService.Send(email, subject, htmlMessage);
        }

        public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }
    }
}
