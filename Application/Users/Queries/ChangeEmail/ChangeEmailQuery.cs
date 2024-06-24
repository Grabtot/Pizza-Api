using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Queries.ChangeEmail
{
    public record ChangeEmailQuery(string OldEmail, string NewEmail) : IRequest<ErrorOr<Success>>;
}
