using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Commands.ConfirmAccount
{
    public record ConfirmAccountCommand(Guid UserId, string Code) : IRequest<ErrorOr<Success>>;
}
