using ErrorOr;
using MediatR;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Commands.ChangeName
{
    public record ChangeNameCommand(Guid UserId, string NewName) : IRequest<ErrorOr<User>>;
}
