namespace PizzaApi.Api.Models.Allergens
{
    public record UpdateAllergenRequest(string CurrentName,
        string? NewName,
        string? Description);
}
