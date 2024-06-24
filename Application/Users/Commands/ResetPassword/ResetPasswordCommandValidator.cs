using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PizzaApi.Application.Common.Validation;

namespace PizzaApi.Application.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator(PasswordOptions passwordOptions)
        {
            RuleFor(c => c.Email).NotNull().EmailAddress();
            RuleFor(c => c.NewPassword).Password(passwordOptions);
        }
    }
}
