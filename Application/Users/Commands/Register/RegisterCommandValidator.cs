using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PizzaApi.Application.Common.Validation;

namespace PizzaApi.Application.Users.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator(PasswordOptions options)
        {
            RuleFor(c => c.Email).EmailAddress().NotNull();
            RuleFor(c => c.Password).Password(options);
            RuleFor(c => c.Name).UserName();
        }
    }
}
