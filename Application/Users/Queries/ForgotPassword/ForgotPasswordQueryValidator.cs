using FluentValidation;

namespace PizzaApi.Application.Users.Queries.ForgotPassword
{
    public class ForgotPasswordQueryValidator : AbstractValidator<ForgotPasswordQuery>
    {
        public ForgotPasswordQueryValidator()
        {
            RuleFor(q => q.Email).NotNull().EmailAddress();
        }
    }
}
