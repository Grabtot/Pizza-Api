using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Application.Users.Queries.ResendConfirmationEmail
{
    public record ResendConfirmationEmailQuery(string Email) : IRequest<ErrorOr<Success>>;
}
