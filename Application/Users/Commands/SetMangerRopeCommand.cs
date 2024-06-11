using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Commands
{
    public record SetMangerRopeCommand(Guid UserId) : IRequest<ErrorOr<Success>>
    {

    }
}
