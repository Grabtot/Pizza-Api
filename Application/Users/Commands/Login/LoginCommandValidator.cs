using FluentValidation;

namespace PizzaApi.Application.Users.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(c => c.Email).EmailAddress().NotNull();
            RuleFor(c => c.Password).NotNull();
        }
    }
}
