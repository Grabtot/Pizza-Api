using FluentEmail.Core;
using PizzaApi.Application.Common.Interfaces;
using Serilog;

namespace PizzaApi.Infrastructure.Services.Email
{
    public class EmailService(IFluentEmailFactory emailFactory) : IEmailService
    {
        private readonly IFluentEmailFactory _emailFactory = emailFactory;

        public async Task Send(string toAddress, string subject, string? body = "")
        {
            Log.Information("Sending email");

            IFluentEmail email = _emailFactory.Create();

            await email.To(toAddress)
                 .Subject(subject)
                 .Body(body, true)
                 .SendAsync();
        }
    }
}
