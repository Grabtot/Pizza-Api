using PizzaApi.Domain.Common.Interfaces;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Domain.Ingredients
{
    public class Ingredient : IAuditableEntity
    {
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Name { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Allergen> Allergens { get; set; }
        public double Price { get; set; }

        public Ingredient()
        {
            Id = Guid.NewGuid();
            Tags = [];
            Allergens = [];
        }
    }
}
