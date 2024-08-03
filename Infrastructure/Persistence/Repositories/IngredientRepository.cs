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
            return await DbSet
                .Include(ingredient => ingredient.Tags)
                .Include(ingredient => ingredient.Allergens)
                .SingleOrDefaultAsync(x => x.Name == name);
        }

        public override Task<Ingredient?> FindAsync(Guid id)
        {
            return DbSet
                .Include(ingredient => ingredient.Tags)
                .Include(ingredient => ingredient.Allergens)
                .FirstOrDefaultAsync(ingredient => ingredient.Id == id);
        }
    }
}
