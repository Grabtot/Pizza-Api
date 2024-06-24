namespace PizzaApi.Api.Models.Users
{
    public class UserResponse
    {
        public Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Email { get; init; }
        public required bool EmailConfirmed { get; init; }
        public string? Role { get; init; }
    }
}
