using FluentValidation;

namespace PizzaApi.Application.Allergens.Commands.CreateAllergen
{
    public class CreateAllergenCommandValidator : AbstractValidator<CreateAllergenCommand>
    {
        public CreateAllergenCommandValidator()
        {
            RuleFor(command => command.Name).NotNull();
        }
    }
}
