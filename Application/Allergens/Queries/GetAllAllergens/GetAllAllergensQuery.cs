using ErrorOr;
using MediatR;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Allergens.Queries.GetAllAllergens
{
    public class GetAllAllergensQuery : IRequest<ErrorOr<List<Allergen>>>
    {
    }
}
