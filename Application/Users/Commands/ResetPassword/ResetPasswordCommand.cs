using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Commands.ResetPassword
{
    public record ResetPasswordCommand(
        string Email,
        string ResetCode,
        string NewPassword) : IRequest<ErrorOr<Success>>;
}
