using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

    }
}
