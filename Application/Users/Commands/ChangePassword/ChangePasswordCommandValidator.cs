using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PizzaApi.Application.Common.Validation;

namespace PizzaApi.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator(PasswordOptions passwordOptions)
        {
            RuleFor(c => c.OldPassword).NotEmpty().NotNull();
            RuleFor(c => c.NewPassword).Password(passwordOptions);
        }
    }
}
