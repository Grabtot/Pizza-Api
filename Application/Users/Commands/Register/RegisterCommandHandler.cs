using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Commands.Register
{
    public class RegisterCommandHandler(UserManager<User> userManager) : IRequestHandler<RegisterCommand, ErrorOr<User>>
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<ErrorOr<User>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            User user = new()
            {
                Name = command.Name,
                UserName = command.Email,
                Email = command.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded)
            {
                IEnumerable<Error> errors = result.Errors
                    .Select(error => Error.Conflict(error.Code, error.Description));

                return errors.ToList();
            }

            return user;
        }
    }
}
