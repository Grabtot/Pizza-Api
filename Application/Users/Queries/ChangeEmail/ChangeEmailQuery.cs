using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Users.Queries.ChangeEmail
{
    public record ChangeEmailQuery(Guid UserId, string NewEmail) : IRequest<ErrorOr<Success>>;
}
