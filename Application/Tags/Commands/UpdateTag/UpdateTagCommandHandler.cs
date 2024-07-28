using ErrorOr;
using MediatR;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Tags.Commands.UpdateTag
{
    public class UpdateTagCommandHandler(ITagRepository tagRepository)
                : IRequestHandler<UpdateTagCommand, ErrorOr<Tag>>
    {
        private readonly ITagRepository _tagRepository = tagRepository;

        public async Task<ErrorOr<Tag>> Handle(UpdateTagCommand command, CancellationToken cancellationToken)
        {
            Tag? tag = await _tagRepository.FindAsync(command.CurrentName);

            if (tag == null)
                return Error.NotFound(description: $"Tag {command.CurrentName} not found");

            if (command.NewName != null && await _tagRepository.FindAsync(command.NewName) != null)
                return Error.Conflict(description: $"Tag {command.NewName} already exists");

            tag.Name = command.NewName ?? tag.Name;
            tag.Color = command.Color;

            return tag;
        }
    }
}
