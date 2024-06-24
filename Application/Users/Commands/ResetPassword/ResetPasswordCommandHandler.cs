using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using PizzaApi.Domain.Users;
using System.Text;

namespace PizzaApi.Application.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler(UserManager<User> userManager)
        : IRequestHandler<ResetPasswordCommand, ErrorOr<Success>>
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<ErrorOr<Success>> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            User? user = await _userManager.FindByEmailAsync(command.Email);

            if (user == null || !user.EmailConfirmed)
                return Error.Unauthorized();

            string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.ResetCode));

            IdentityResult result = await _userManager.ResetPasswordAsync(user, code, command.NewPassword);

            if (!result.Succeeded)
            {
                List<Error> errors = result.Errors
                    .Select(error => Error.Unexpected(error.Code, error.Description))
                    .ToList();

                return errors;
            }

            return Result.Success;
        }
    }
}
