using FluentValidation;
using PizzaApi.Application.Common.Validation;

namespace PizzaApi.Application.Users.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(c => c.Email).EmailAddress().NotNull();
            RuleFor(c => c.Password).NotNull().NotEmpty();
            RuleFor(c => c.Name).UserName();
        }
    }
}
