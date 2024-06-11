﻿using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Users;
using PizzaApi.Infrastructure.Authentication;
using PizzaApi.Infrastructure.Common.Interfaces;
using PizzaApi.Infrastructure.Common.Options;
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


            services.AddRepositories();

            services.AddFluentEmail(configuration);
            services.AddAuthenticationAndAuthorization(configuration);

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
            // services.AddTransient<IEmailSender<User>, EmailSender>();

            Log.Debug("Email options = {@emailSettings}", options);

            return services;
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

        public static WebApplication MigrateDatabase(this WebApplication host, IConfiguration configuration)
        {
            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                try
                {
                    PizzaDbContext context = services.GetRequiredService<PizzaDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An error occurred while applying migrations.");
                    throw;
                }
            }

            if (host.Environment.IsDevelopment() && configuration.GetValue<bool>("SeedDatabase"))
            {
                using (IServiceScope scope = host.Services.CreateScope())
                {
                    UserManager<User> userManager = scope.ServiceProvider
                        .GetRequiredService<UserManager<User>>();

                    DatabaseSeeder seeder = new(userManager);
                    seeder.Seed();
                }
            }

            return host;
        }
    }
}
