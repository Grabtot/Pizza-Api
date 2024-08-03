using PizzaApi.Api.Models.Allergens;
using PizzaApi.Api.Models.Tags;

namespace PizzaApi.Api.Models.Ingredients
{
    public record IngredientDetailsResponse(
        Guid Id,
        string Name,
        decimal Price,
        List<TagResponse>? Tags,
        List<AllergenResponse>? Allergens);
}
