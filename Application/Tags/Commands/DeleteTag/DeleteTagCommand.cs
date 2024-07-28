using ErrorOr;
using MediatR;

namespace PizzaApi.Application.Tags.Commands.DeleteTag
{
    public record DeleteTagCommand(string Name)
        : IRequest<ErrorOr<Success>>;
}
