using MediatR;
using Microsoft.AspNetCore.Identity;

namespace PizzaApi.Application.Users.Queries.PasswordRequirements
{
    public record PasswordRequirementsQuery : IRequest<PasswordOptions>
    {
    }
}
