using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler(UserManager<User> userManager)
        : IRequestHandler<ChangePasswordCommand, ErrorOr<Success>>
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<ErrorOr<Success>> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            User? user = await _userManager.FindByIdAsync(command.UserId.ToString())
                ?? throw new InvalidOperationException($"User with email {command.UserId} not found");

            IdentityResult result = await _userManager.ChangePasswordAsync(user, command.OldPassword, command.NewPassword);

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
