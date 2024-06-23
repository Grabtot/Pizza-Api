using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Commands.ConfirmNewEmail
{
    public record ConfirmNewEmailCommand(Guid UserId,
        string Code,
        string Email) : IRequest<ErrorOr<Success>>;
}
