using Microsoft.EntityFrameworkCore;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Infrastructure.Persistence.Repositories
{
    public class AllergensRepository(PizzaDbContext context)
        : Repository<Allergen, string>(context), IAllergenRepository
    {
        public override Task<Allergen?> FindAsync(string id)
        {
            return base.FindAsync(id.ToUpper());
        }

        public async Task<List<Allergen>> GetAll(CancellationToken cancellationToken = default)
        {
            return await DbSet.ToListAsync(cancellationToken);
        }
    }
}
