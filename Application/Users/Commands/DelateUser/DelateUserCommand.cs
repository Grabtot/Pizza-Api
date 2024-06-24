using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Commands.DelateUser
{
    public record DelateUserCommand(Guid UserId) : IRequest<ErrorOr<Success>>;
}
