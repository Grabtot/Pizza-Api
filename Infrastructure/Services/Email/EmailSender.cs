using Microsoft.AspNetCore.Identity.UI.Services;
using PizzaApi.Infrastructure.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
