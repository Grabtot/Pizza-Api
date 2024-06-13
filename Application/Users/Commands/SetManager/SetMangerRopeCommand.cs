using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Commands.SetManager
{
    public record SetMangerRopeCommand(Guid UserId) : IRequest<ErrorOr<Success>>
    {

    }
}
