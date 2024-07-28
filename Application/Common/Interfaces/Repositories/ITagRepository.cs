using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Common.Interfaces.Repositories
{
    public interface ITagRepository : IRepository<Tag, string>
    {
        new Task<Tag?> FindAsync(string name);
        Task<List<Tag>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
