using Microsoft.AspNetCore.Identity;
using PizzaApi.Domain.Common.Interfaces;

namespace PizzaApi.Domain.Users
{
    public class User : IdentityUser<Guid>, IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Name { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
