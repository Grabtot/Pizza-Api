using Microsoft.EntityFrameworkCore;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Infrastructure.Persistence.Repositories
{
    public class AllergensRepository(PizzaDbContext context)
        : Repository<Allergen, string>(context), IAllergenRepository
    {
        public override async Task<Allergen?> FindAsync(string name)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.NormalizedName == name.ToUpper());
        }

        public async Task<List<Allergen>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.ToListAsync(cancellationToken);
        }
    }
}
