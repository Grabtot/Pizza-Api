using FluentValidation;

namespace PizzaApi.Application.Users.Queries.ResendConfirmationEmail
{
    public class ResendConfirmationEmailQueryValidator : AbstractValidator<ResendConfirmationEmailQuery>
    {
        public ResendConfirmationEmailQueryValidator()
        {
            RuleFor(q => q.Email).NotNull().EmailAddress();
        }
    }
}
