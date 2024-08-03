using PizzaApi.Api.Models.Allergens;
using PizzaApi.Api.Models.Tags;

namespace PizzaApi.Api.Models.Ingredients
{
    public record CreateIngredientRequest(
        string Name,
        decimal Price,
        List<CreateTagRequest>? Tags = null,
        List<CreateAllergenRequest>? Allergens = null);
}
