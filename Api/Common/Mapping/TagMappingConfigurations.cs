using Mapster;
using PizzaApi.Api.Models.Tags;
using PizzaApi.Application.Tags.Commands.CreateTag;
using PizzaApi.Application.Tags.Commands.UpdateTag;
using PizzaApi.Domain.Ingredients.ValueObjects;
using System.Drawing;

namespace PizzaApi.Api.Common.Mapping
{
    public class TagMappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateTagRequest, CreateTagCommand>()
                .Map(command => command.Color,
                    request => request.Color.HasValue ?
                        Color.FromArgb(request.Color.Value) : (Color?)null);

            config.NewConfig<Tag, TagResponse>()
                .Map(response => response.Color, tag => tag.Color.GetValueOrDefault().ToArgb());

            config.NewConfig<UpdateTagRequest, UpdateTagCommand>()
                .Map(command => command.Color,
                    request => request.Color.HasValue ?
                        Color.FromArgb(request.Color.Value) : (Color?)null);
        }
    }
}
