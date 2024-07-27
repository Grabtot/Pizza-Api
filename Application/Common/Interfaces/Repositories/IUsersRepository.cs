using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Common.Interfaces.Repositories
{
    public interface IUsersRepository : IRepository<User, Guid>
    {
    }
}
