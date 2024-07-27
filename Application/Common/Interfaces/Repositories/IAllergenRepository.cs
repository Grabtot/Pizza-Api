using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Common.Interfaces.Repositories
{
    public interface IAllergenRepository : IRepository<Allergen, string>
    {
    }
}
