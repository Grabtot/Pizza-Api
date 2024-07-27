using ErrorOr;
using MediatR;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Allergens.Commands.UpdateAllergen
{
    public record UpdateAllergenCommand(string CurrentName,
        string? NewName,
        string? Description) : IRequest<ErrorOr<Allergen>>;
}
