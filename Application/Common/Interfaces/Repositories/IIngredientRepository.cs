using PizzaApi.Domain.Ingredients;

namespace PizzaApi.Application.Common.Interfaces.Repositories
{
    public interface IIngredientRepository : IRepository<Ingredient, Guid>
    {
        Task<Ingredient?> FindByNameAsync(string name);
    }
}
