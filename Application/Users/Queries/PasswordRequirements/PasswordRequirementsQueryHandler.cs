using MediatR;
using Microsoft.AspNetCore.Identity;

namespace PizzaApi.Application.Users.Queries.PasswordRequirements
{
    public class PasswordRequirementsQueryHandler(PasswordOptions options)
        : IRequestHandler<PasswordRequirementsQuery, PasswordOptions>
    {
        private readonly PasswordOptions _options = options;

        public async Task<PasswordOptions> Handle(PasswordRequirementsQuery query, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_options);
        }
    }
}
