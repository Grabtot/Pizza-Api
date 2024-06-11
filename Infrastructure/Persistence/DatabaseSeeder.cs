using Microsoft.AspNetCore.Identity;
using PizzaApi.Application.Common.Constants;
using PizzaApi.Domain.Users;
using Serilog;

namespace PizzaApi.Infrastructure.Persistence
{
    internal class DatabaseSeeder
    {
        private readonly UserManager<User> _userManager;

        public DatabaseSeeder(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public void Seed()
        {
            Log.Information("Start seeding database");

            string email = "test@user.com";
            User? user = _userManager.FindByEmailAsync(email).Result;

            if (user != null)
            {
                Log.Information($"{email} already exists");
                return;
            }

            user = new()
            {
                Id = Guid.NewGuid(),
                UserName = "Grabtot",
                Email = email,
                CreatedAt = DateTime.UtcNow
            };

            _userManager.CreateAsync(user, "pass").Wait();
            _userManager.AddToRoleAsync(user, Constants.Account.Developer).Wait();

            Log.Information($"{email} created");
        }
    }
}
