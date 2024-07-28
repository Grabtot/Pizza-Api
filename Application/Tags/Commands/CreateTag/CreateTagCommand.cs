using ErrorOr;
using MediatR;
using PizzaApi.Domain.Ingredients.ValueObjects;
using System.Drawing;

namespace PizzaApi.Application.Tags.Commands.CreateTag
{
    public record CreateTagCommand(string Name, Color? Color)
        : IRequest<ErrorOr<Tag>>;
}
