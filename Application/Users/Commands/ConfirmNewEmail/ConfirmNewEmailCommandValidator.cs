using FluentValidation;

namespace PizzaApi.Application.Users.Commands.ConfirmNewEmail
{
    public class ConfirmNewEmailCommandValidator : AbstractValidator<ConfirmNewEmailCommand>
    {
        public ConfirmNewEmailCommandValidator()
        {
            RuleFor(c => c.Email).NotNull().EmailAddress();
        }
    }
}
