using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PizzaApi.Application.Common.Behaviors;
using System.Reflection;

namespace PizzaApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediator();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(
                    Assembly.GetExecutingAssembly());

                options.AddOpenBehavior(typeof(LoggingBehavior<,>));
                options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            return services;
        }

    }
}
