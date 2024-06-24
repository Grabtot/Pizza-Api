using Mapster;
using PizzaApi.Api.Models.Users;
using PizzaApi.Domain.Users;

namespace PizzaApi.Api.Common.Mapping
{
    public class UserMappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(User User, string? Role), UserResponse>()
                .Map(response => response, src => src.User)
                .Map(response => response.Role, srs => srs.Role);
        }
    }
}
