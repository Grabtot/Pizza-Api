using FluentValidation;

namespace PizzaApi.Application.Users.Commands.SetManager
{
    public class SetMangerRopeCommandValidator : AbstractValidator<SetMangerRopeCommand>
    {
        public SetMangerRopeCommandValidator()
        {
            RuleFor(c => c.Email).NotNull().EmailAddress();
        }
    }
}
