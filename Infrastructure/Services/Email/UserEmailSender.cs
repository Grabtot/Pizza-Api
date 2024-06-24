using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using PizzaApi.Domain.Users;

namespace PizzaApi.Infrastructure.Services.Email
{
    public class UserEmailSender(IEmailSender emailSender) : IEmailSender<User>
    {
        private readonly IEmailSender _emailSender = emailSender;

        public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        {
            string htmlMessage = $"Hi, {user.UserName}! Please confirm your account by " +
                $"<a href=\"{confirmationLink}\">clicking here</a>.";

            await _emailSender.SendEmailAsync(email, "Confirm your email", htmlMessage);
        }

        public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            string htmlMessage = $"Please reset your password using the following code: {resetCode}";

            await _emailSender.SendEmailAsync(email, "Reset your password", htmlMessage);
        }

        public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        {
            string htmlMessage = $"Please reset your password by <a href=\"{resetLink}\"> clicking here </a>.";

            await _emailSender.SendEmailAsync(email, "Reset your password", htmlMessage);
        }
    }
}
