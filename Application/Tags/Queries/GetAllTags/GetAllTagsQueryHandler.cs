using MediatR;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Tags.Queries.GetAllTags
{
    public class GetAllTagsQueryHandler(ITagRepository tagRepository)
        : IRequestHandler<GetAllTagsQuery, List<Tag>>
    {
        private readonly ITagRepository _tagRepository = tagRepository;

        public async Task<List<Tag>> Handle(GetAllTagsQuery query, CancellationToken cancellationToken)
        {
            return await _tagRepository.GetAllAsync(cancellationToken);
        }
    }
}
