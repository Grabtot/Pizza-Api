using ErrorOr;
using MediatR;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Allergens.Commands.CreateAllergen
{
    public record CreateAllergenCommand(string Name, string? Description)
        : IRequest<ErrorOr<Allergen>>;
}
