using ErrorOr;
using Moq;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Application.Tags.Commands.DeleteTag;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Tests.Application.Tags
{
    public class DeleteTagCommandHandlerTests
    {
        private readonly Mock<ITagRepository> _mockTagRepository;
        private readonly DeleteTagCommandHandler _handler;

        public DeleteTagCommandHandlerTests()
        {
            _mockTagRepository = new Mock<ITagRepository>();
            _handler = new DeleteTagCommandHandler(_mockTagRepository.Object);
        }

        [Theory]
        [MemberData(nameof(ValidDeleteTagCommands))]
        public async Task HandleDeleteTagCommand_TagExists_ShouldDeleteTag(DeleteTagCommand command)
        {
            // Arrange
            Tag existingTag = new(command.Name);
            _mockTagRepository.Setup(repo => repo.FindAsync(command.Name))
                .ReturnsAsync(existingTag);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(Result.Success, result.Value);
            _mockTagRepository.Verify(repo => repo.Delete(existingTag), Times.Once);
        }

        [Theory]
        [MemberData(nameof(ValidDeleteTagCommands))]
        public async Task HandleDeleteTagCommand_TagDoesNotExist_ShouldReturnNotFound(DeleteTagCommand command)
        {
            // Arrange
            _mockTagRepository.Setup(repo => repo.FindAsync(command.Name))
                .ReturnsAsync((Tag)null);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains(result.Errors, e => e.Code == Error.NotFound().Code);
            _mockTagRepository.Verify(repo => repo.Delete(It.IsAny<Tag>()), Times.Never);
        }

        public static IEnumerable<object[]> ValidDeleteTagCommands()
        {
            yield return new object[] { new DeleteTagCommand("Meat") };
            yield return new object[] { new DeleteTagCommand("Fish") };
        }
    }
}
