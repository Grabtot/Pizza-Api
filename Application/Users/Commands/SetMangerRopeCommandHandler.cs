using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PizzaApi.Application.Common.Constants;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Commands
{
    public class SetMangerRopeCommandHandler(UserManager<User> userManager)
        : IRequestHandler<SetMangerRopeCommand, ErrorOr<Success>>
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<ErrorOr<Success>> Handle(SetMangerRopeCommand command, CancellationToken cancellationToken)
        {
            Guid userId = command.UserId;
            User? user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return Error.NotFound(description: $"User with id {userId} not found");
            }

            IdentityResult result = await _userManager.AddToRoleAsync(user, Constants.Role.Manger);

            if (!result.Succeeded)
            {
                IdentityError error = result.Errors.First();
                throw new InvalidOperationException(error.Description);
            }

            return Result.Success;
        }
    }
}
