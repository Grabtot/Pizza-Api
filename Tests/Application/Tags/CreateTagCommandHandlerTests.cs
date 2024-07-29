using ErrorOr;
using Moq;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Application.Tags.Commands.CreateTag;
using PizzaApi.Domain.Ingredients.ValueObjects;
using System.Drawing;

namespace PizzaApi.Tests.Application.Tags
{
    public class CreateTagCommandHandlerTests
    {
        private readonly Mock<ITagRepository> _mockTagRepository;
        private readonly CreateTagCommandHandler _handler;

        public CreateTagCommandHandlerTests()
        {
            _mockTagRepository = new Mock<ITagRepository>();
            _handler = new CreateTagCommandHandler(_mockTagRepository.Object);
        }

        [Theory]
        [MemberData(nameof(ValidCreateTagCommands))]
        public async Task HandleCreateTagCommand_TagDoesNotExist_ShouldReturnTag(CreateTagCommand command)
        {
            // Arrange
            _mockTagRepository.Setup(repo => repo.FindAsync(command.Name))
                .ReturnsAsync((Tag)null);

            // Act
            ErrorOr<Tag> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.NotNull(result.Value);
            Assert.Equal(command.Name, result.Value.Name);
            Assert.Equal(command.Color, result.Value.Color);
            _mockTagRepository.Verify(repo => repo.AddAsync(It.IsAny<Tag>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(InvalidCreateTagCommands))]
        public async Task HandleCreateTagCommand_TagAlreadyExists_ShouldReturnConflictError(CreateTagCommand command)
        {
            // Arrange
            Tag existingTag = new(command.Name, command.Color);
            _mockTagRepository.Setup(repo => repo.FindAsync(command.Name))
                .ReturnsAsync(existingTag);

            // Act
            ErrorOr<Tag> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains(result.Errors, e => e.Code == Error.Conflict().Code);
            _mockTagRepository.Verify(repo => repo.AddAsync(It.IsAny<Tag>()), Times.Never);
        }

        public static IEnumerable<object[]> ValidCreateTagCommands()
        {
            yield return new object[] { new CreateTagCommand("Vegetables", Color.Green) };
            yield return new object[] { new CreateTagCommand("Fruits", Color.Yellow) };
            yield return new object[] { new CreateTagCommand("Dairy", Color.White) };
        }

        public static IEnumerable<object[]> InvalidCreateTagCommands()
        {
            yield return new object[] { new CreateTagCommand("Meat", Color.Red) }; // Assuming "Meat" already exists
            yield return new object[] { new CreateTagCommand("Fish", Color.Blue) }; // Assuming "Fish" already exists
        }
    }
}
