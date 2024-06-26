using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PizzaApi.Domain.Common.Interfaces;
using PizzaApi.Domain.Users;

namespace PizzaApi.Infrastructure.Persistence
{
    public class PizzaDbContext(DbContextOptions<PizzaDbContext> options)
        : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(PizzaDbContext).Assembly);

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry> entities = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity &&
                (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (EntityEntry entityEntry in entities)
            {
                IAuditableEntity entity = (IAuditableEntity)entityEntry.Entity;
                entity.UpdatedAt = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
