using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid UserId) : IRequest<ErrorOr<Success>>;
}
