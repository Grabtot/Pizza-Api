using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApi.Domain.Ingredients;
using PizzaApi.Domain.Ingredients.ValueObjects;
using System.Drawing;

namespace PizzaApi.Infrastructure.Persistence.Configurations
{
    internal class TagConfigurations : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(tag => tag.NormalizedName);

            builder.HasIndex(tag => tag.Name).IsUnique();

            //builder.HasMany<Ingredient>();

            builder.Property(tag => tag.Color)
                .HasConversion(
                color => TryGetArgb(color),
                argb => TryGetColor(argb));
        }

        private int? TryGetArgb(Color? color) => color?.ToArgb();
        private Color? TryGetColor(int? argb) => argb != null ? Color.FromArgb(argb.Value) : null;
    }
}
