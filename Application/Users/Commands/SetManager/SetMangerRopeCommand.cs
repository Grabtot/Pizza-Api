using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Commands.SetManager
{
    public record SetMangerRopeCommand(string Email) : IRequest<ErrorOr<Success>>
    {

    }
}
