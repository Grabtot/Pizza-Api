using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(UserManager<User> userManager)
        : IRequestHandler<DeleteUserCommand, ErrorOr<Success>>
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<ErrorOr<Success>> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
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
