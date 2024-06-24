using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Queries.ForgotPassword
{
    public record ForgotPasswordQuery(string Email) : IRequest<ErrorOr<Success>>;
}
