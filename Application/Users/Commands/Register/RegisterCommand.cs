using ErrorOr;
using MediatR;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Commands.Register
{
    public record RegisterCommand(
        string Email,
        string Name,
        string Password) :
        IRequest<ErrorOr<User>>;
}
