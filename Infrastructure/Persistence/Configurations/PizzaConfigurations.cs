using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApi.Domain.Pizzas;

namespace PizzaApi.Infrastructure.Persistence.Configurations
{
    internal class PizzaConfigurations : IEntityTypeConfiguration<Pizza>
    {
        public void Configure(EntityTypeBuilder<Pizza> builder)
        {
            // builder.HasMany<Ingredient>(nameof(Pizza.Ingredients));
        }
    }
}
