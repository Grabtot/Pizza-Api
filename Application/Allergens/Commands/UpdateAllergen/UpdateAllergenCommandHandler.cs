using ErrorOr;
using MediatR;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Allergens.Commands.UpdateAllergen
{
    public class UpdateAllergenCommandHandler(IAllergenRepository allergenRepository)
        : IRequestHandler<UpdateAllergenCommand, ErrorOr<Allergen>>
    {
        private readonly IAllergenRepository _allergenRepository = allergenRepository;

        public async Task<ErrorOr<Allergen>> Handle(UpdateAllergenCommand command, CancellationToken cancellationToken)
        {
            Allergen? allergen = await _allergenRepository.FindAsync(command.CurrentName);

            if (allergen == null)
                return Error.NotFound(description: $"Allergen {command.CurrentName} not found");

            if (command.NewName != null && await _allergenRepository.FindAsync(command.NewName) != null)
                return Error.Conflict(description: $"Allergen {command.NewName} already exists");

            allergen.Name = command.NewName ?? allergen.Name;
            allergen.Description = command.Description;

            return allergen;
        }
    }
}
