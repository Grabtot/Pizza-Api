using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Allergens.Commands.DeleteAllergen
{
    public record DeleteAllergenCommand(string Name) : IRequest<ErrorOr<Success>>
    {
    }
}
