using ErrorOr;
using MediatR;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Allergens.Queries.GetAllAllergens
{
    public class GetAllAllergensQueryHandler(IAllergenRepository allergenRepository)
        : IRequestHandler<GetAllAllergensQuery, ErrorOr<List<Allergen>>>
    {
        private readonly IAllergenRepository _allergenRepository = allergenRepository;

        public async Task<ErrorOr<List<Allergen>>> Handle(GetAllAllergensQuery query, CancellationToken cancellationToken)
        {
            List<Allergen> allergens = await _allergenRepository.GetAll(cancellationToken);

            return allergens;
        }
    }
}
