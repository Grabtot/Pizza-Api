using ErrorOr;
using MediatR;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Application.Tags.Commands.DeleteTag
{
    public class DeleteTagCommandHandler(ITagRepository tagRepository)
        : IRequestHandler<DeleteTagCommand, ErrorOr<Success>>
    {
        private readonly ITagRepository _tagRepository = tagRepository;

        public async Task<ErrorOr<Success>> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
        {
            Tag? tag = await _tagRepository.FindAsync(command.Name);

            if (tag == null)
                return Error.NotFound(description: $"Tag {command.Name} not found");

            _tagRepository.Delete(tag);

            return Result.Success;
        }
    }
}
