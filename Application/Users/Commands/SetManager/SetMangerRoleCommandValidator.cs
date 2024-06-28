using FluentValidation;

namespace PizzaApi.Application.Users.Commands.SetManager
{
    public class SetMangerRoleCommandValidator : AbstractValidator<SetMangerRoleCommand>
    {
        public SetMangerRoleCommandValidator()
        {
            RuleFor(c => c.Email).NotNull().EmailAddress();
        }
    }
}
