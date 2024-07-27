using ErrorOr;
using MediatR;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Allergens.Commands.DeleteAllergen
{
    public class DeleteAllergenCommandHandler(IAllergenRepository allergenRepository)
        : IRequestHandler<DeleteAllergenCommand, ErrorOr<Success>>
    {
        private readonly IAllergenRepository _allergenRepository = allergenRepository;

        public async Task<ErrorOr<Success>> Handle(DeleteAllergenCommand command, CancellationToken cancellationToken)
        {
            Allergen? allergen = await _allergenRepository.FindAsync(command.Name);

            if (allergen == null)
                return Error.NotFound(description: $"Allergen {command.Name} not found");

            _allergenRepository.Delate(allergen);

            return Result.Success;
        }
    }
}
