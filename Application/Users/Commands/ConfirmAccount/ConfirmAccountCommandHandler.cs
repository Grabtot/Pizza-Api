using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using PizzaApi.Domain.Users;
using System.Text;

namespace PizzaApi.Application.Users.Commands.ConfirmAccount
{
    public class ConfirmAccountCommandHandler(UserManager<User> userManager)
        : IRequestHandler<ConfirmAccountCommand, ErrorOr<Success>>
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<ErrorOr<Success>> Handle(ConfirmAccountCommand command, CancellationToken cancellationToken)
        {
            User? user = await _userManager.FindByIdAsync(command.UserId.ToString());

            if (user == null)
                return Error.Unauthorized();

            string decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.Code));

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, decodedCode);

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
