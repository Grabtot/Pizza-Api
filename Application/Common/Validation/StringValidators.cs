using FluentValidation;

namespace PizzaApi.Application.Common.Validation
{
    public static class StringValidators
    {
        /// <summary>
        ///    Checks if string is not null, not empty and not white space and has length between minimum and maximum
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumLength"></param>
        /// <param name="minimumLength"></param>
        public static IRuleBuilderOptions<T, string> RequiredLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maximumLength, int minimumLength = 1)
        {
            return ruleBuilder.NotNull().NotEmpty().Length(minimumLength, maximumLength);

        }

        /// <summary>
        /// Checks if string is not null, not empty and not white space and has length not greater than maximum
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumLength"></param>
        public static IRuleBuilderOptions<T, string> RequiredMaximumLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maximumLength)
        {
            return ruleBuilder.RequiredLength(maximumLength);
        }

        /// <summary>
        /// Checks if string is not null, not empty and not white space and has length not less than minimum
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minimumLength"></param>
        public static IRuleBuilderOptions<T, string> RequiredMinimumLength<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength)
        {
            return ruleBuilder.NotNull().NotEmpty().MinimumLength(minimumLength);
        }
    }
}
