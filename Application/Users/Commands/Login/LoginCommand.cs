using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Commands.Login
{
    public record LoginCommand(
        string Email,
        string Password,
        bool? UseCookies,
        bool? UseSessionCookies) : IRequest<ErrorOr<Success>>;

}
