using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Application.Users.Commands
{
    public record SetMangerRopeCommand(Guid UserId) : IRequest<ErrorOr<Success>>
    {

    }
}
