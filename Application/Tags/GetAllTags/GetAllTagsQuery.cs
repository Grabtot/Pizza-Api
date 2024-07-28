using MediatR;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Tags.GetAllTags
{
    public record GetAllTagsQuery : IRequest<List<Tag>>;
}
