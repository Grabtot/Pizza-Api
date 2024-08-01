using PizzaApi.Domain.Common.Interfaces;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Domain.Ingredients
{
    public class Ingredient : IAuditableEntity
    {
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Allergen> Allergens { get; set; }
        public double Price { get; set; }

#pragma warning disable CS8618 
        private Ingredient() { }
#pragma warning restore CS8618

        public Ingredient(string name, double price, List<Tag> tags, List<Allergen> allergens)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Tags = tags;
            Allergens = allergens;
        }
    }
}
