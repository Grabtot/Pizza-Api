using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Commands.ChangePassword
{
    public record ChangePasswordCommand(Guid UserId,
        string OldPassword,
        string NewPassword) : IRequest<ErrorOr<Success>>;
}
