using Microsoft.AspNetCore.Identity;

namespace PizzaApi.Domain.Users
{
    public class User : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
