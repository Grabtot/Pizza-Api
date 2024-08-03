using PizzaApi.Domain.Common.Interfaces;
using System.Drawing;

namespace PizzaApi.Domain.Ingredients.ValueObjects
{
    public class Tag : IAuditableEntity
    {
        private string _name;
        public Guid Id { get; private set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NormalizedName = value.ToUpper();
            }
        }

        public string NormalizedName { get; private set; }
        public Color? Color { get; set; }

#pragma warning disable CS8618 
        private Tag() { }
#pragma warning restore CS8618

        public Tag(string name, Color? color = null)
        {
            Id = Guid.NewGuid();
            _name = name;
            NormalizedName = name.ToUpper();
            Color = color;
        }

        public static readonly Tag Meat = new("Meat", System.Drawing.Color.Red);
        public static readonly Tag Fish = new("Fish", System.Drawing.Color.Blue);

        public override int GetHashCode()
            => HashCode.Combine(NormalizedName);

        public override bool Equals(object? obj)
            => obj is Tag other && other == this;

        public static bool operator ==(Tag? x, Tag? y)
            => x?.NormalizedName == y?.NormalizedName;

        public static bool operator !=(Tag? x, Tag? y) => !(x == y);

    }
}
