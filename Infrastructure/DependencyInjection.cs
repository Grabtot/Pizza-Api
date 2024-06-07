using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Infrastructure.Persistence;
using PizzaApi.Infrastructure.Persistence.Repositories;
using Serilog;

namespace PizzaApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (connectionString == null)
            {
                Log.Fatal("No connection string provided");
            }

            services.AddDbContext<PizzaDbContext>(options
                => options.UseNpgsql(connectionString));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }
    }
}
