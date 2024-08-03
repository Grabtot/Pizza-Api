using Mapster;
using PizzaApi.Api.Models.Allergens;
using PizzaApi.Api.Models.Ingredients;
using PizzaApi.Api.Models.Tags;
using PizzaApi.Application.Allergens.Commands.CreateAllergen;
using PizzaApi.Application.Ingredients.Commands.CreateIngredient;
using PizzaApi.Domain.Ingredients;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Api.Common.Mapping
{
    public class IngredientMappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateIngredientRequest, CreateIngredientCommand>()
                .MapWith(request => MapFromRequest(request));

            config.NewConfig<Ingredient, IngredientDetailsResponse>()
                .Map(response => response.Tags, ingredient
                    => ingredient.Tags.ConvertAll(tag => tag.Adapt<TagResponse>()))
                .Map(response => response.Allergens, ingredient
                    => ingredient.Allergens.ConvertAll(allergen => allergen.Adapt<AllergenResponse>()));
        }

        private CreateIngredientCommand MapFromRequest(CreateIngredientRequest request)
        {
            List<Tag> tags = request.Tags?.ConvertAll(t => new Tag(t.Name)) ?? [];
            List<Allergen> allergens = request.Allergens?.ConvertAll(a => new Allergen(a.Name)) ?? [];

            return new CreateIngredientCommand(request.Name, request.Price, tags, allergens);
        }
    }
}
