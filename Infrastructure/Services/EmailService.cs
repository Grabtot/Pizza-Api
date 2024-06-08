using FluentEmail.Core;
using PizzaApi.Infrastructure.Common.Interfaces;
using Serilog;

namespace PizzaApi.Infrastructure.Services
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
