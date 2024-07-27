using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Common.Interfaces.Repositories
{
    public interface IAllergenRepository : IRepository<Allergen, string>
    {
        new Task<Allergen?> FindAsync(string name);
        Task<List<Allergen>> GetAll(CancellationToken cancellationToken = default);
    }
}
