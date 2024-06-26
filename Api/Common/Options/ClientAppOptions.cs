namespace PizzaApi.Api.Common.Options
{
    public class ClientAppOptions
    {
        public const string SectionName = "ClientAppOptions";

        public required string BaseUrl { get; init; }
        public required string ConfirmationPath { get; init; }
        public required string PasswordResetPath { get; init; }
        public bool UseClientUrisInEmails { get; init; }
    }
}
