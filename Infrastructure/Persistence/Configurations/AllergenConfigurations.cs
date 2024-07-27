using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApi.Domain.Ingredients;
using PizzaApi.Domain.Ingredients.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Infrastructure.Persistence.Configurations
{
    internal class AllergenConfigurations : IEntityTypeConfiguration<Allergen>
    {
        public void Configure(EntityTypeBuilder<Allergen> builder)
        {
            builder.HasKey("_id");
            builder.Property("_id").ValueGeneratedNever();

            builder.HasIndex(allergen => allergen.Name).IsUnique();
            builder.HasIndex(allergen => allergen.NormalizedName).IsUnique();
        }
    }
}
