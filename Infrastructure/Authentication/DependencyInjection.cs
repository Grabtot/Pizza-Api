using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using PizzaApi.Application.Common.Constants;
using PizzaApi.Domain.Users;
using PizzaApi.Infrastructure.Authentication;
using PizzaApi.Infrastructure.Persistence;
using Serilog;

namespace PizzaApi.Infrastructure.Authentication
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Extends the service collection to add authentication and authorization settings.
        /// </summary>
        /// <param name="services">The service collection for configuring the application.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The service collection with added authentication and authorization settings.</returns>
        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(configuration);
            services.AddAuthorization();

            return services;
        }

        /// <summary>
        /// Configures authentication and identity services for the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance used to retrieve configuration settings.</param>
        /// <returns>The <see cref="IServiceCollection"/> with added authentication services.</returns>
        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentity(options =>
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

            services.AddAuthentication(Constants.Account.BearerAndApplication)
              .AddScheme<AuthenticationSchemeOptions, CompositeHandler>(Constants.Account.BearerAndApplication, null, options =>
              {
                  options.ForwardDefault = Constants.Account.Bearer;
                  options.ForwardAuthenticate = Constants.Account.BearerAndApplication;
              })
              .AddBearerToken(Constants.Account.Bearer)
              .AddIdentityCookies();

            return services;
        }

        /// <summary>
        /// Configures authentication and identity services for the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance used to retrieve configuration settings.</param>
        /// <returns>The <see cref="IServiceCollection"/> with added authentication services.</returns>
        private static IServiceCollection AddAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(Constants.Account.MangerOrDeveloper, policyBuilder =>
                {
                    policyBuilder.RequireRole(Constants.Account.Manger, Constants.Account.Developer);
                });

            return services;
        }

        /// <summary>
        /// Attempts to retrieve identity option sections from the configuration.
        /// </summary>
        /// <typeparam name="TOptions">The type of the identity options.</typeparam>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance used to retrieve configuration settings.</param>
        /// <returns>The retrieved identity options, or <c>null</c> if not specified in the configuration.</returns>
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

        /// <summary>
        /// Configures the identity services for the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="setupAction">An action to configure the <see cref="IdentityOptions"/>.</param>
        /// <returns>An <see cref="IdentityBuilder"/> that can be used to further configure identity services.</returns>
        private static IdentityBuilder AddIdentity(this IServiceCollection services, Action<IdentityOptions> setupAction)
        {
            services.AddHttpContextAccessor();

            services.AddSingleton(TimeProvider.System);
            services.AddDataProtection();
            services.TryAddScoped<IUserValidator<User>, UserValidator<User>>();
            services.TryAddScoped<IPasswordValidator<User>, PasswordValidator<User>>();
            services.TryAddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator<IdentityRole<Guid>>, RoleValidator<IdentityRole<Guid>>>();

            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<User>>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<SecurityStampValidatorOptions>, PostConfigureSecurityStampValidatorOptions>());
            services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<User>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, IdentityRole<Guid>>>();
            services.TryAddScoped<IUserConfirmation<User>, DefaultUserConfirmation<User>>();
            services.TryAddScoped<UserManager<User>>();
            services.TryAddScoped<SignInManager<User>>();
            services.TryAddScoped<RoleManager<IdentityRole<Guid>>>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new IdentityBuilder(typeof(User), typeof(IdentityRole<Guid>), services);
        }

    }
}
