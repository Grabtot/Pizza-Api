using ErrorOr;
using MediatR;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Application.Users.Queries
{
    public class GetUserQueryHandler(IUsersRepository usersRepository)
        : IRequestHandler<GetUserQuery, ErrorOr<User>>
    {
        private readonly IUsersRepository _usersRepository = usersRepository;

        public async Task<ErrorOr<User>> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.FindAsync(query.Id);

            if (user is null)
            {
                return Error.NotFound(description: $"User with id {query.Id} not found");
            }

            return user;
        }
    }
}
