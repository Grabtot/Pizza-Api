using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PizzaApi.Application.Common.Interfaces;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Queries.ForgotPassword
{
    public class ForgotPasswordQueryHandler(
        UserManager<User> userManager,
        IAccountEmailSender emailSender) : IRequestHandler<ForgotPasswordQuery, ErrorOr<Success>>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IAccountEmailSender _emailSender = emailSender;

        public async Task<ErrorOr<Success>> Handle(ForgotPasswordQuery query, CancellationToken cancellationToken)
        {
            User? user = await _userManager.FindByEmailAsync(query.Email);

            if (user == null || !user.EmailConfirmed)
                return Error.Unauthorized();

            await _emailSender.SendPasswordRecoveryEmailAsync(user, true);
            return Result.Success;
        }
    }
}
