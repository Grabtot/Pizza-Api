using MediatR;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Tags.Queries.GetAllTags
{
    public record GetAllTagsQuery : IRequest<List<Tag>>;
}
