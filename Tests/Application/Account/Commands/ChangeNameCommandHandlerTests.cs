using ErrorOr;
using Moq;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Application.Users.Commands.ChangeName;
using PizzaApi.Domain.Users;
using PizzaApi.Tests.Application.Account.Commands.TestUtils;
using PizzaApi.Tests.Application.TestUtils.Constants;

namespace PizzaApi.Tests.Application.Account.Commands
{
    public class ChangeNameCommandHandlerTests
    {
        private readonly Mock<IUsersRepository> _mockUsersRepository;
        private readonly ChangeNameCommandHandler _handler;

        public ChangeNameCommandHandlerTests()
        {
            _mockUsersRepository = new Mock<IUsersRepository>();
            _handler = new ChangeNameCommandHandler(_mockUsersRepository.Object);
        }

        [Fact]
        public async Task HandleChangeNameCommand_UserExists_ShouldReturnUserWithNewName()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new()
            { Id = userId, Name = Constants.User.Name };
            string newName = "New Name";

            _mockUsersRepository.Setup(repo => repo.FindAsync(userId))
                .ReturnsAsync(user);

            ChangeNameCommand command = CommandsCreateUtils.CreateChangeNameCommand(userId, newName);

            // Act
            ErrorOr<User> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(newName, result.Value.Name);
        }

        [Fact]
        public async Task HandleChangeNameCommand_UserNotFound_ShouldThrowInvalidOperationException()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            string newName = "New Name";

            _mockUsersRepository.Setup(repo => repo.FindAsync(userId))
                .ReturnsAsync((User)null);

            ChangeNameCommand command = CommandsCreateUtils.CreateChangeNameCommand(userId, newName);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _handler.Handle(command, CancellationToken.None));
        }
    }
}
