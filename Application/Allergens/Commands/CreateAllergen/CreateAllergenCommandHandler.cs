using ErrorOr;
using MediatR;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Allergens.Commands.CreateAllergen
{
    public class CreateAllergenCommandHandler(IAllergenRepository repository)
        : IRequestHandler<CreateAllergenCommand, ErrorOr<Allergen>>
    {
        private readonly IAllergenRepository _allergenRepository = repository;

        public async Task<ErrorOr<Allergen>> Handle(CreateAllergenCommand command, CancellationToken cancellationToken)
        {
            Allergen? allergen = await _allergenRepository.FindAsync(command.Name);

            if (allergen != null)
                return Error.Conflict(description: $"Allergen with the name {command.Name} already exists");

            allergen = new(command.Name, command.Description);

            await _allergenRepository.AddAsync(allergen);

            return allergen;
        }
    }
}
