namespace PizzaApi.Api.Models.Users
{
    public record ResetPasswordRequest(string Email, string ResetCode, string NewPassword);
}
