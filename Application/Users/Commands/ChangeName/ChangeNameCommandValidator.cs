using FluentValidation;
using PizzaApi.Application.Common.Validation;

namespace PizzaApi.Application.Users.Commands.ChangeName
{
    public class ChangeNameCommandValidator : AbstractValidator<ChangeNameCommand>
    {
        public ChangeNameCommandValidator()
        {
            RuleFor(c => c.NewName).UserName();
        }
    }
}
