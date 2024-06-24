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

                bool isId = Guid.TryParse(id, out Guid userId);

                return isId ? userId : null;
            }
        }
        public string? UserName => _httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        public string? Role => _httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        public string? Email => _httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
    }
}
