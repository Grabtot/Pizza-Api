using FluentValidation;

namespace PizzaApi.Application.Users.Queries.ChangeEmail
{
    public class ChangeEmailQueryValidator : AbstractValidator<ChangeEmailQuery>
    {
        public ChangeEmailQueryValidator()
        {
            RuleFor(c => c.OldEmail).NotNull().EmailAddress();
            RuleFor(c => c.NewEmail).NotNull().EmailAddress();
        }
    }
}
