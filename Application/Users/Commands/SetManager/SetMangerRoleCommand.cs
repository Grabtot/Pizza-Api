using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Commands.SetManager
{
    public record SetMangerRoleCommand(string Email) : IRequest<ErrorOr<Success>>
    {

    }
}
