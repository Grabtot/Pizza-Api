using ErrorOr;
using MediatR;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommandHandler(
        IIngredientRepository ingredientRepository,
        IAllergenRepository allergenRepository,
        ITagRepository tagRepository) : IRequestHandler<CreateIngredientCommand, ErrorOr<Ingredient>>
    {
        private readonly IIngredientRepository _ingredientRepository = ingredientRepository;
        private readonly ITagRepository _tagRepository = tagRepository;
        private readonly IAllergenRepository _allergenRepository = allergenRepository;

        public async Task<ErrorOr<Ingredient>> Handle(CreateIngredientCommand command, CancellationToken cancellationToken)
        {
            Ingredient? ingredient = await _ingredientRepository.FindByNameAsync(command.Name);

            if (ingredient != null)
                return Error.Conflict(description: $"Ingredient {command.Name} already exists");

            List<Error> errors = [];

            ErrorOr<Success> tagsCheckResult = await CheckAllTagsExists(command, cancellationToken);

            ErrorOr<Success> allergensCheckResult = await CheckAllAllergensExists(command, cancellationToken);

            if (tagsCheckResult.IsError || allergensCheckResult.IsError)
            {
                errors.AddRange(tagsCheckResult.Errors);
                errors.AddRange(allergensCheckResult.Errors);

                return errors;
            }

            List<Tag> tags = command.Tags.ConvertAll(t => _tagRepository.FindAsync(t.Name).Result)!;
            List<Allergen> allergens = command.Allergens.ConvertAll(a => _allergenRepository.FindAsync(a.Name).Result)!;

            ingredient = new(command.Name, command.Price, tags, allergens);
            await _ingredientRepository.AddAsync(ingredient);

            return ingredient;
        }

        private async Task<ErrorOr<Success>> CheckAllAllergensExists(CreateIngredientCommand command,
            CancellationToken cancellationToken)
        {
            List<Error> errors = [];
            if (command.Allergens.Count > 0)
            {
                List<Allergen> allergens = await _allergenRepository.GetAllAsync(cancellationToken);
                if (!command.Allergens.All(allergens.Contains))
                {
                    IEnumerable<Allergen> missingAllergens = command.Allergens
                        .Where(allergen => !allergens.Contains(allergen));

                    foreach (Allergen allergen in missingAllergens)
                    {
                        Error error = Error.Validation("Ingredient.Allergens", $"Allergen {allergen.Name} does not exist");
                        errors.Add(error);
                    }

                    return errors;
                }
            }



            return Result.Success;
        }

        private async Task<ErrorOr<Success>> CheckAllTagsExists(CreateIngredientCommand command,
            CancellationToken cancellationToken)
        {
            List<Error> errors = [];
            if (command.Tags.Count > 0)
            {
                List<Tag> tags = await _tagRepository.GetAllAsync(cancellationToken);
                if (!command.Tags.All(tags.Contains))
                {
                    IEnumerable<Tag> missingTags = command.Tags
                        .Where(tag => !tags.Contains(tag));

                    foreach (Tag tag in missingTags)
                    {
                        Error error = Error.Validation("Ingredient.Tags", $"Tag {tag.Name} does not exist");
                        errors.Add(error);
                    }

                    return errors;
                }
            }

            return Result.Success;
        }
    }
}
