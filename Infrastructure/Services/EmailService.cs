using FluentEmail.Core;
using PizzaApi.Infrastructure.Common.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Infrastructure.Services
{
    public class EmailService(IFluentEmailFactory emailFactory) : IEmailService
    {
        private readonly IFluentEmailFactory _emailFactory = emailFactory;

        public async Task Send(string toAddress, string subject, string? body = "")
        {
            Log.Information("Sending email");

            var email = _emailFactory.Create();

            await email.To(toAddress)
                 .Subject(subject)
                 .Body(body, true)
                 .SendAsync();
        }
    }
}
