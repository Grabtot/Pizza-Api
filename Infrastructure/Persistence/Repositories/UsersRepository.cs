using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Users;

namespace PizzaApi.Infrastructure.Persistence.Repositories
{
    public class UsersRepository(PizzaDbContext context)
        : Repository<User>(context), IUsersRepository
    {
    }
}
