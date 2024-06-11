using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PizzaApi.Application.Common.Constants;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace PizzaApi.Infrastructure.Authentication
{
    public class CompositeHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
       : SignInAuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult bearerResult = await Context.AuthenticateAsync(Constants.Account.Bearer);

            if (!bearerResult.None)
            {
                return bearerResult;
            }

            return await Context.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        }

        protected override Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleSignOutAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }
    }
}
