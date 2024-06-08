using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Users;
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

            services.AddAuthentication(configuration);

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityApiEndpoints<User>(options =>
            {
                PasswordOptions? passwordOptions = TryGetIdentityOptionsSections<PasswordOptions>(configuration);
                SignInOptions? signInOptions = TryGetIdentityOptionsSections<SignInOptions>(configuration);
                LockoutOptions? lockoutOptions = TryGetIdentityOptionsSections<LockoutOptions>(configuration);
                UserOptions? userOptions = TryGetIdentityOptionsSections<UserOptions>(configuration);
                ClaimsIdentityOptions? claimsIdentityOptions = TryGetIdentityOptionsSections<ClaimsIdentityOptions>(configuration);
                TokenOptions? tokenOptions = TryGetIdentityOptionsSections<TokenOptions>(configuration);
                StoreOptions? storeOptions = TryGetIdentityOptionsSections<StoreOptions>(configuration);

                options.Password = passwordOptions ?? options.Password;
                options.SignIn = signInOptions ?? options.SignIn;
                options.Lockout = lockoutOptions ?? options.Lockout;
                options.User = userOptions ?? options.User;
                options.ClaimsIdentity = claimsIdentityOptions ?? options.ClaimsIdentity;
                options.Tokens = tokenOptions ?? options.Tokens;
                options.Stores = storeOptions ?? options.Stores;

                Log.Debug("Identity options = {@Options}", options);
            })
            .AddEntityFrameworkStores<PizzaDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }

        private static TOptions? TryGetIdentityOptionsSections<TOptions>(IConfiguration configuration)
        {
            string sectionName = typeof(TOptions).Name;

            TOptions? options = configuration.GetSection(
                $"IdentityOptions:{sectionName}").Get<TOptions>();

            if (options == null)
            {
                Log.Warning($"{sectionName} identity options is not specified");
            }

            return options;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }
    }
}
