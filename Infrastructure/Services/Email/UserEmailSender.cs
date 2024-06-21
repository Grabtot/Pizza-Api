using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using PizzaApi.Domain.Users;
using PizzaApi.Infrastructure.Common.Interfaces;

namespace PizzaApi.Infrastructure.Services.Email
{
    public class UserEmailSender(IEmailSender emailSender) : IEmailSender<User>
    {
        private readonly IEmailSender _emailSender = emailSender;

        public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        {
            string htmlMEssage = $"Hi, {user.UserName}! Please confirm your account by " +
                $"<a href='{confirmationLink}'>clicking here</a>.";

            await _emailSender.SendEmailAsync(email, "Confirm your email", htmlMEssage);
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
