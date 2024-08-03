using PizzaApi.Domain.Common.Interfaces;

namespace PizzaApi.Domain.Ingredients.ValueObjects
{
    public class Allergen : IAuditableEntity
    {
        private string _name;
        private Guid _id;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name
        {
            get => _name; set
            {
                _name = value;
                NormalizedName = value.ToUpper();
            }
        }
        public string? Description { get; set; }
        public string NormalizedName { get; private set; }

#pragma warning disable CS8618 
        private Allergen() { }
#pragma warning restore CS8618

        public Allergen(string name, string? description = null)
        {
            _id = Guid.NewGuid();
            _name = name;
            NormalizedName = name.ToUpper();
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

        public override string ToString()
        {
            return $"{Name}, {_id}";
        }
    }
}
