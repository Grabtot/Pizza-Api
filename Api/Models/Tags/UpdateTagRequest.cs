namespace PizzaApi.Api.Models.Tags
{
    public record UpdateTagRequest(string CurrentName,
        string? NewName,
        int? Color);
}
