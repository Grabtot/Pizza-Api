using FluentValidation;
using static PizzaApi.Application.Common.Constants.Constants;

namespace PizzaApi.Application.Common.Validation
{
    public static class UserNameValidator
    {
        public static IRuleBuilderOptions<T, string> UserName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .RequiredLength(Account.MaxUserNameLength, Account.MinUserNameLength)
                .NotNull();
        }
    }
}
