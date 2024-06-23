using Microsoft.AspNetCore.Identity.UI.Services;

namespace PizzaApi.Infrastructure.Services.Email
{
    public class FakeEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage) => Task.CompletedTask;
    }
}
