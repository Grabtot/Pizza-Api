using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PizzaApi.Application.Common.Validation
{
    public static partial class PasswordValidatorExtentions
    {
        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, PasswordOptions options)
        {
            return ruleBuilder.SetValidator(new PasswordValidator(options));
        }

        private static IRuleBuilderOptions<T, string> RequireNonAlphanumeric<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(NonAlphanumericRegex())
                               .WithMessage("The password must contain at least one non-alphanumeric character.");
        }

        private static IRuleBuilderOptions<T, string> RequiredUniqueChars<T>(this IRuleBuilder<T, string> ruleBuilder, int count)
        {
            return ruleBuilder.Must(password => ContainsMinimumUniqueCharacters(password, count))
                               .WithMessage($"The password must contain at least {count} unique characters.");
        }

        private static IRuleBuilderOptions<T, string> RequireLowercase<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(LowercaseRegex())
                               .WithMessage("The password must contain at least one lowercase letter.");
        }

        private static IRuleBuilderOptions<T, string> RequireUppercase<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(UppercaseRegex())
                               .WithMessage("The password must contain at least one uppercase letter.");
        }

        private static IRuleBuilderOptions<T, string> RequireDigit<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(DigitRegex())
                               .WithMessage("The password must contain at least one digit.");
        }

        private static bool ContainsMinimumUniqueCharacters(string input, int minUniqueChars)
        {
            HashSet<char> uniqueChars = new(input);

            return uniqueChars.Count >= minUniqueChars;
        }


        [GeneratedRegex(@"\W")]
        private static partial Regex NonAlphanumericRegex();

        [GeneratedRegex(@"(?=\p{Ll})")]
        private static partial Regex LowercaseRegex();

        [GeneratedRegex(@"(?=\p{Lu})")]
        private static partial Regex UppercaseRegex();

        [GeneratedRegex(@"\d")]
        private static partial Regex DigitRegex();

        private partial class PasswordValidator : AbstractValidator<string>
        {

            public PasswordValidator(PasswordOptions passwordOptions)
            {
                PasswordOptions options = passwordOptions;

                RuleFor(p => p).RequiredMinimumLength(options.RequiredLength)
                    .WithMessage($"The password must contain at least {options.RequiredLength} characters.");
                RuleFor(p => p).RequiredUniqueChars(options.RequiredUniqueChars);

                if (options.RequireLowercase)
                    RuleFor(p => p).RequireLowercase();

                if (options.RequireUppercase)
                    RuleFor(p => p).RequireUppercase();

                if (options.RequireDigit)
                    RuleFor(p => p).RequireDigit();

                if (options.RequireNonAlphanumeric)
                    RuleFor(p => p).RequireNonAlphanumeric();
            }
        }
    }
}
