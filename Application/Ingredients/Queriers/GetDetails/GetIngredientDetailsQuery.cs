using ErrorOr;
using MediatR;
using PizzaApi.Domain.Ingredients;

namespace PizzaApi.Application.Ingredients.Queriers.GetDetails
{
    public record GetIngredientDetailsQuery(Guid Id)
        : IRequest<ErrorOr<Ingredient>>;
}
