using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Commands.DelateUser
{
    public class DelateUserCommandHandler(UserManager<User> userManager)
        : IRequestHandler<DelateUserCommand, ErrorOr<Success>>
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<ErrorOr<Success>> Handle(DelateUserCommand command, CancellationToken cancellationToken)
        {
            User user = await _userManager.FindByIdAsync(command.UserId.ToString())
                ?? throw new InvalidOperationException($"User with id {command.UserId} not found");

            IdentityResult result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            return Result.Success;
        }
    }
}
