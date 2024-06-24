using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PizzaApi.Application.Common.Interfaces;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Queries.ChangeEmail
{
    public class ChangeEmailQueryHandler(
        IAccountEmailSender emailSender,
        UserManager<User> userManager) : IRequestHandler<ChangeEmailQuery, ErrorOr<Success>>
    {
        private readonly IAccountEmailSender _emailSender = emailSender;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<ErrorOr<Success>> Handle(ChangeEmailQuery command, CancellationToken cancellationToken)
        {
            User? user = await _userManager.FindByEmailAsync(command.OldEmail);

            if (user is null)
                return Error.Unauthorized();

            User? newEmailUser = await _userManager.FindByEmailAsync(command.NewEmail);
            if (newEmailUser != null)
            {
                return Error.Conflict(description: $"Email {command.NewEmail} already taken");
            }

            await _emailSender.SendConfirmationEmailAsync(user, command.NewEmail, true);

            return Result.Success;
        }
    }
}
