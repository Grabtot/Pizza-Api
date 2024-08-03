using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApi.Domain.Ingredients.ValueObjects;
using System.Drawing;

namespace PizzaApi.Infrastructure.Persistence.Configurations
{
    internal class TagConfigurations : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(tag => tag.Id).ValueGeneratedNever();

            builder.HasIndex(tag => tag.Name).IsUnique();
            builder.HasIndex(tag => tag.NormalizedName).IsUnique();

            builder.Property(tag => tag.Color)
                .HasConversion(
                color => TryGetArgb(color),
                argb => TryGetColor(argb));
        }

        private int? TryGetArgb(Color? color) => color?.ToArgb();
        private Color? TryGetColor(int? argb) => argb != null ? Color.FromArgb(argb.Value) : null;
    }
}
