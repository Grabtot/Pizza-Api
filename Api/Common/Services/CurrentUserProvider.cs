using PizzaApi.Application.Common.Interfaces;
using System.Security.Claims;

namespace PizzaApi.Api.Common.Services
{
    public class CurrentUserProvider(IHttpContextAccessor contextAccessor) : ICurrentUserProvider
    {
        private readonly HttpContext _httpContext = contextAccessor.HttpContext
            ?? throw new ArgumentNullException("HttpContext", "Http Context is null");

        public Guid? UserId
        {
            get
            {
                string? id = _httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                Guid.TryParse(id, out Guid userId);

                return userId;
            }
        }
        public string? UserName => _httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
    }
}
