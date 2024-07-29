using ErrorOr;
using Moq;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Application.Tags.Commands.UpdateTag;
using PizzaApi.Domain.Ingredients.ValueObjects;
using System.Drawing;

namespace PizzaApi.Tests.Application.Tags
{
    public class UpdateTagCommandHandlerTests
    {
        private readonly Mock<ITagRepository> _mockTagRepository;
        private readonly UpdateTagCommandHandler _handler;

        public UpdateTagCommandHandlerTests()
        {
            _mockTagRepository = new Mock<ITagRepository>();
            _handler = new UpdateTagCommandHandler(_mockTagRepository.Object);
        }

        [Theory]
        [MemberData(nameof(ValidUpdateTagCommands))]
        public async Task HandleUpdateTagCommand_TagExists_ShouldUpdateTag(UpdateTagCommand command)
        {
            // Arrange
            Tag existingTag = new(command.CurrentName, Color.Green);
            _mockTagRepository.Setup(repo => repo.FindAsync(command.CurrentName))
                .ReturnsAsync(existingTag);
            _mockTagRepository.Setup(repo => repo.FindAsync(command.NewName))
                .ReturnsAsync((Tag)null);

            // Act
            ErrorOr<Tag> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.NotNull(result.Value);
            Assert.Equal(command.NewName ?? command.CurrentName, result.Value.Name);
            Assert.Equal(command.Color, result.Value.Color);
        }

        [Theory]
        [MemberData(nameof(ValidUpdateTagCommands))]
        public async Task HandleUpdateTagCommand_TagDoesNotExist_ShouldReturnNotFound(UpdateTagCommand command)
        {
            // Arrange
            _mockTagRepository.Setup(repo => repo.FindAsync(command.CurrentName))
                .ReturnsAsync((Tag)null);

            // Act
            ErrorOr<Tag> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains(result.Errors, e => e.Code == Error.NotFound().Code);
        }

        [Theory]
        [MemberData(nameof(ConflictingUpdateTagCommands))]
        public async Task HandleUpdateTagCommand_NewNameAlreadyExists_ShouldReturnConflict(UpdateTagCommand command)
        {
            // Arrange
            Tag existingTag = new(command.CurrentName, Color.Green);
            Tag conflictingTag = new(command.NewName, Color.Red);
            _mockTagRepository.Setup(repo => repo.FindAsync(command.CurrentName))
                .ReturnsAsync(existingTag);
            _mockTagRepository.Setup(repo => repo.FindAsync(command.NewName))
                .ReturnsAsync(conflictingTag);

            // Act
            ErrorOr<Tag> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains(result.Errors, e => e.Code == Error.Conflict().Code);
        }

        public static IEnumerable<object[]> ValidUpdateTagCommands()
        {
            yield return new object[] { new UpdateTagCommand("Vegetables", "Greens", Color.Green) };
            yield return new object[] { new UpdateTagCommand("Fruits", null, Color.Yellow) };
            yield return new object[] { new UpdateTagCommand("Dairy", "Milk Products", null) };
            yield return new object[] { new UpdateTagCommand("Bakery", null, null) };
        }

        public static IEnumerable<object[]> ConflictingUpdateTagCommands()
        {
            yield return new object[] { new UpdateTagCommand("Vegetables", "Meat", Color.Green) }; // Assuming "Meat" already exists
            yield return new object[] { new UpdateTagCommand("Fruits", "Fish", Color.Yellow) }; // Assuming "Fish" already exists
        }
    }
}
