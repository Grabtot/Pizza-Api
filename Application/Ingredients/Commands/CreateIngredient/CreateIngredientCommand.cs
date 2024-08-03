using ErrorOr;
using MediatR;
using PizzaApi.Domain.Ingredients;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Ingredients.Commands.CreateIngredient
{
    public record CreateIngredientCommand(
        string Name,
        decimal Price,
        List<Tag> Tags,
        List<Allergen> Allergens) : IRequest<ErrorOr<Ingredient>>;
}
