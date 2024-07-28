using ErrorOr;
using MediatR;
using PizzaApi.Domain.Ingredients.ValueObjects;
using System.Drawing;

namespace PizzaApi.Application.Tags.Commands.UpdateTag
{
    public record UpdateTagCommand(
        string CurrentName,
        string? NewName,
        Color? Color) : IRequest<ErrorOr<Tag>>;
}
