using ErrorOr;
using MediatR;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommandHandler(ITagRepository tagRepository)
                : IRequestHandler<CreateTagCommand, ErrorOr<Tag>>
    {
        private readonly ITagRepository _tagRepository = tagRepository;

        public async Task<ErrorOr<Tag>> Handle(CreateTagCommand command, CancellationToken cancellationToken)
        {
            Tag? tag = await _tagRepository.FindAsync(command.Name);

            if (tag != null)
                return Error.Conflict(description: $"Tag {command.Name} already exists");

            tag = new(command.Name, command.Color);
            await _tagRepository.AddAsync(tag);

            return tag;
        }
    }
}
