using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PizzaApi.Application.Common.Constants;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Commands.SetManager
{
    public class SetMangerRopeCommandHandler(UserManager<User> userManager)
        : IRequestHandler<SetMangerRopeCommand, ErrorOr<Success>>
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<ErrorOr<Success>> Handle(SetMangerRopeCommand command, CancellationToken cancellationToken)
        {
            User? user = await _userManager.FindByEmailAsync(command.Email);

            if (user is null)
                return Error.NotFound(description: $"User with email {command.Email} not found");


            IdentityResult result = await _userManager.AddToRoleAsync(user, Constants.Account.Manger);

            if (!result.Succeeded)
            {
                IdentityError error = result.Errors.First();
                throw new InvalidOperationException(error.Description);
            }

            return Result.Success;
        }
    }
}
