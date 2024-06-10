using ErrorOr;
using MediatR;
using PizzaApi.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Application.Users.Queries
{
    public record GetUserQuery(Guid Id) : IRequest<ErrorOr<User>>;
}
