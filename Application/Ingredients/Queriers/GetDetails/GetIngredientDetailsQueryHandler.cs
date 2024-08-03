using ErrorOr;
using MediatR;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients;

namespace PizzaApi.Application.Ingredients.Queriers.GetDetails
{
    public class GetIngredientDetailsQueryHandler(IIngredientRepository ingredientRepository)
                : IRequestHandler<GetIngredientDetailsQuery, ErrorOr<Ingredient>>
    {
        private readonly IIngredientRepository _ingredientRepository = ingredientRepository;

        public async Task<ErrorOr<Ingredient>> Handle(GetIngredientDetailsQuery query, CancellationToken cancellationToken)
        {
            Ingredient? ingredient = await _ingredientRepository.FindAsync(query.Id);

            if (ingredient == null)
                return Error.NotFound(description: $"Ingredient with id {query.Id} is not found");

            return ingredient;
        }
    }
}
