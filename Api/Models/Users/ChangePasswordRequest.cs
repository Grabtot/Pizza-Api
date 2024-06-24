namespace PizzaApi.Api.Models.Users
{
    public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
}
