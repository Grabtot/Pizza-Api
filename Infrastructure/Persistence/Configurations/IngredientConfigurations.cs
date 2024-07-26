using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApi.Domain.Ingredients;
using PizzaApi.Domain.Pizzas;

namespace PizzaApi.Infrastructure.Persistence.Configurations
{
    internal class IngredientConfigurations : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            // builder.HasMany<Pizza>();
            builder.HasMany(ing => ing.Tags).WithMany();
            builder.HasMany(ing => ing.Allergens).WithMany();
        }
    }
}
