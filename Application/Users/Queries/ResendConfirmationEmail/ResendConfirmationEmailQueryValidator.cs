using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Application.Users.Queries.ResendConfirmationEmail
{
    public class ResendConfirmationEmailQueryValidator : AbstractValidator<ResendConfirmationEmailQuery>
    {
        public ResendConfirmationEmailQueryValidator()
        {
            RuleFor(q => q.Email).NotNull().EmailAddress();
        }
    }
}
