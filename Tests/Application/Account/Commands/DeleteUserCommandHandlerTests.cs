using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Moq;
using PizzaApi.Application.Users.Commands.DeleteUser;
using PizzaApi.Domain.Users;
using PizzaApi.Tests.Application.TestUtils.Constants;

namespace PizzaApi.Tests.Application.Account.Commands
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly DeleteUserCommandHandler _handler;

        public DeleteUserCommandHandlerTests()
        {
            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            _handler = new DeleteUserCommandHandler(_mockUserManager.Object);
        }

        [Fact]
        public async Task HandleDeleteUserCommand_UserExists_ShouldReturnSuccess()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new()
            { Id = userId, Name = Constants.User.Name };

            _mockUserManager.Setup(um => um.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            DeleteUserCommand command = new(userId);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(Result.Success, result.Value);
        }

        [Fact]
        public async Task HandleDeleteUserCommand_UserNotFound_ShouldThrowInvalidOperationException()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            _mockUserManager.Setup(um => um.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((User)null);

            DeleteUserCommand command = new(userId);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task HandleDeleteUserCommand_DeleteFailed_ShouldThrowException()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new()
            { Id = userId, Name = Constants.User.Name };

            _mockUserManager.Setup(um => um.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Delete failed" }));

            DeleteUserCommand command = new(userId);

            // Act & Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(async () =>
                await _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Delete failed", exception.Message);
        }
    }
}
