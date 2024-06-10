using Mapster;
using PizzaApi.Api.Models.Users;
using PizzaApi.Domain.Users;

namespace PizzaApi.Api.Common.Mapping
{
    public class UserMappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, UserResponse>()
                .Map(response => response.Name, user => user.UserName)
                .Map(response => response, user => user);
        }
    }
}
