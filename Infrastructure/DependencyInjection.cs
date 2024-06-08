using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Users;
using PizzaApi.Infrastructure.Common.Interfaces;
using PizzaApi.Infrastructure.Common.Interfaces.Options;
using PizzaApi.Infrastructure.Persistence;
using PizzaApi.Infrastructure.Persistence.Repositories;
using PizzaApi.Infrastructure.Services;
using Serilog;
using System.Net;
using System.Net.Mail;

namespace PizzaApi.Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds infrastructure services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance used to retrieve configuration settings.</param>
        /// <returns>The <see cref="IServiceCollection"/> with added infrastructure services.</returns>
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
            services.AddFluentEmail(configuration);

            return services;
        }

        private static IServiceCollection AddFluentEmail(this IServiceCollection services,
            IConfiguration configuration)
        {
            EmailOptions? options = configuration.GetSection(EmailOptions.SectionName).Get<EmailOptions>();

            if (options == null || options.UseEmailSender == false)
            {
                Log.Information("Email sender disabled");
                return services;
            }

            services.AddFluentEmail(options.DefaultFromEmail);

            NetworkCredential? credentials = null;

            if (!string.IsNullOrEmpty(options.Username) && !string.IsNullOrEmpty(options.Password))
            {
                credentials = new NetworkCredential(options.Username, options.Password);
            }

            SmtpSender sender = new(new SmtpClient()
            {
                Host = options.Host,
                Port = options.Port,
                Credentials = credentials
            });

            services.AddSingleton<ISender>(sender);

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEmailSender, EmailSender>();

            Log.Debug("Email options = {@emailSettings}", options);

            return services;
        }

        /// <summary>
        /// Configures authentication and identity services for the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance used to retrieve configuration settings.</param>
        /// <returns>The <see cref="IServiceCollection"/> with added authentication services.</returns>
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
        /// Adds repository services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> with added repository services.</returns>
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }
    }
}
