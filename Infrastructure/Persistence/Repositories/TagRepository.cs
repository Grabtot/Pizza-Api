using Microsoft.EntityFrameworkCore;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Infrastructure.Persistence.Repositories
{
    public class TagRepository(PizzaDbContext context)
        : Repository<Tag, string>(context), ITagRepository
    {
        public override async Task<Tag?> FindAsync(string name)
        {
            return await DbSet.FirstOrDefaultAsync(tag
                => tag.NormalizedName == name.ToUpper());
        }

        public async Task<List<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.ToListAsync(cancellationToken);
        }
    }
}
