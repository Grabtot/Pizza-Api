using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using PizzaApi.Api.Controllers;
using PizzaApi.Application.Common.Interfaces;
using PizzaApi.Domain.Users;
using System.Text;
using System.Text.Encodings.Web;

namespace PizzaApi.Api.Common.Services
{
    public class AccountEmailSender(UserManager<User> userManager,
        LinkGenerator linkGenerator,
        IHttpContextAccessor contextAccessor,
        IEmailSender<User> emailSender) : IAccountEmailSender
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IEmailSender<User> _emailSender = emailSender;
        private readonly LinkGenerator _linkGenerator = linkGenerator;
        private readonly HttpContext _httpContext = contextAccessor.HttpContext
            ?? throw new ArgumentNullException();

        public async Task SendConfirmationEmailAsync(User user, string email, bool isChange = false)
        {
            string code = isChange
                ? await _userManager.GenerateChangeEmailTokenAsync(user, email)
                : await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            RouteValueDictionary routeValues = new()
            {
                ["userId"] = user.Id,
                ["code"] = code,
            };

            if (isChange)
                routeValues.Add("changedEmail", email);

            string? confirmEmailUrl = _linkGenerator.GetUriByAction(_httpContext,
                nameof(AccountController.ConfirmEmail),
                "account", routeValues)
                ?? throw new NotSupportedException($"Could not find endpoint named '{nameof(AccountController.ConfirmEmail)}'.");

            await _emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
        }
    }
}
