using PizzaApi.Domain.Common.Interfaces;

namespace PizzaApi.Domain.Pizzas
{
    public class Pizza : IAuditableEntity
    {
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public double Price { get; set; }
        public required string Name { get; set; }
        public List<Guid> Ingredients { get; set; }

        public Pizza()
        {
            Id = Guid.NewGuid();
            Ingredients = [];
        }

    }
}
