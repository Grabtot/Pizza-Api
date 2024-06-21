using Microsoft.AspNetCore.Identity;
using PizzaApi.Application.Common.Constants;
using PizzaApi.Domain.Users;
using Serilog;

namespace PizzaApi.Infrastructure.Persistence
{
    internal class DatabaseSeeder(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;

        public void Seed()
        {
            Log.Information("Start seeding database");

            SeedRoles();

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
                Name = "Grabtot",
                UserName = email,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _userManager.CreateAsync(user, "pass").Wait();
            _userManager.AddToRoleAsync(user, Constants.Account.Developer).Wait();

            Log.Information($"{email} created");
            Log.Information($"Database seeded");

        }

        private void SeedRoles()
        {
            Log.Information("Seeding roles");
            string managerRoleName = Constants.Account.Manger;

            IdentityRole<Guid>? mangaer = _roleManager.FindByNameAsync(managerRoleName).Result;

            if (mangaer != null)
            {
                Log.Information($"{managerRoleName} role exists");
            }
            else
            {
                mangaer = new(managerRoleName);
                _roleManager.CreateAsync(mangaer).Wait();

                Log.Information($"{managerRoleName} role created");
            }

            string developerRoleName = Constants.Account.Developer;

            IdentityRole<Guid>? developer = _roleManager.FindByNameAsync(developerRoleName).Result;

            if (developer != null)
            {
                Log.Information($"{developerRoleName} role exists");
            }
            else
            {
                developer = new(developerRoleName);
                _roleManager.CreateAsync(developer).Wait();

                Log.Information($"{developerRoleName} role created");
            }

            Log.Information("Roles created");
        }
    }
}
