using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace PizzaApi.Application.Users.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<ErrorOr<ClaimsPrincipal>>;
}
