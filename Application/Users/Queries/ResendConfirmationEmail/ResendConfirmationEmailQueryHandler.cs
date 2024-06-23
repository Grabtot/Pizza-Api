using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PizzaApi.Application.Common.Interfaces;
using PizzaApi.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Application.Users.Queries.ResendConfirmationEmail
{
    public class ResendConfirmationEmailQueryHandler(
        UserManager<User> userManager,
        IAccountEmailSender accountEmailSender) : IRequestHandler<ResendConfirmationEmailQuery, ErrorOr<Success>>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IAccountEmailSender _accountEmailSender = accountEmailSender;
        public async Task<ErrorOr<Success>> Handle(ResendConfirmationEmailQuery query, CancellationToken cancellationToken)
        {
            User? user = await _userManager.FindByEmailAsync(query.Email);

            if (user == null)
            {
                return Error.NotFound();
            }

            await _accountEmailSender.SendConfirmationEmailAsync(user, query.Email);
            return Result.Success;
        }
    }
}
