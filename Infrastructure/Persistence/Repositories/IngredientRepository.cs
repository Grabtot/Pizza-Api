using Microsoft.EntityFrameworkCore;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients;

namespace PizzaApi.Infrastructure.Persistence.Repositories
{
    public class IngredientRepository(PizzaDbContext context)
        : Repository<Ingredient, Guid>(context), IIngredientRepository
    {
        public async Task<Ingredient?> FindByNameAsync(string name)
        {
            return await DbSet.SingleOrDefaultAsync(x => x.Name == name);
        }
    }
}
