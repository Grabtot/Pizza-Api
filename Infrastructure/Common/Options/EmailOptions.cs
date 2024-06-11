namespace PizzaApi.Infrastructure.Common.Options
{
    public class EmailOptions
    {
        public const string SectionName = "EmailOptions";
        public required bool UseEmailSender { get; init; } = true;
        public required string Host { get; init; }
        public string? Password { get; init; }
        public string? Username { get; init; }
        public required string DefaultFromEmail { get; init; }
        public required int Port { get; init; }

    }
}
