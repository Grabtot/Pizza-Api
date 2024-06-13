using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PizzaApi.Application.Common.Constants;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Commands.Login
{
    public class LoginCommandHandler(SignInManager<User> signInManager) : IRequestHandler<LoginCommand, ErrorOr<Success>>
    {
        private readonly SignInManager<User> _signInManager = signInManager;

        public async Task<ErrorOr<Success>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            bool userCookieScheme = command.UseCookies == true || command.UseSessionCookies == true;
            bool isPersistent = (command.UseCookies == true) && (command.UseSessionCookies != true);

            _signInManager.AuthenticationScheme = userCookieScheme ? IdentityConstants.ApplicationScheme
                : Constants.Account.Bearer;

            User? user = await _signInManager.UserManager.FindByEmailAsync(command.Email);

            if (user == null)
            {
                return Error.Unauthorized();
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, command.Password, isPersistent, false);

            if (!result.Succeeded)
            {
                return Error.Unauthorized();
            }

            return Result.Success;
        }
    }
}
