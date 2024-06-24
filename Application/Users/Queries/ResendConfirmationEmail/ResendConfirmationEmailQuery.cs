using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Queries.ResendConfirmationEmail
{
    public record ResendConfirmationEmailQuery(string Email) : IRequest<ErrorOr<Success>>;
}
