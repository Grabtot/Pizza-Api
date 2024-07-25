using PizzaApi.Domain.Common.Interfaces;

namespace PizzaApi.Domain.Ingredients.ValueObjects
{
    public class Allergen : IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        private string NormalizedName => Name.ToUpper();

#pragma warning disable CS8618 
        private Allergen() { }
#pragma warning restore CS8618

        public Allergen(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NormalizedName, Description);
        }

        public override bool Equals(object? obj)
            => obj is Allergen other && other == this;

        public static bool operator ==(Allergen? x, Allergen? y)
            => x?.NormalizedName == y?.NormalizedName
            && x?.Description == y?.Description;

        public static bool operator !=(Allergen? x, Allergen? y) => !(x == y);
    }
}
