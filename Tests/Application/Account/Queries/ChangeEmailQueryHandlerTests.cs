using ErrorOr;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PizzaApi.Application.Common.Interfaces;
using PizzaApi.Application.Users.Queries.ChangeEmail;
using PizzaApi.Domain.Users;
using PizzaApi.Tests.Application.TestUtils.Constants;

namespace Tests.Application.Users.Queries
{
    public class ChangeEmailQueryHandlerTests
    {
        private readonly Mock<IAccountEmailSender> _mockEmailSender;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly ChangeEmailQueryHandler _handler;

        public ChangeEmailQueryHandlerTests()
        {
            _mockEmailSender = new Mock<IAccountEmailSender>();

            Mock<IUserStore<User>> userStoreMock = new();

            _mockUserManager = new Mock<UserManager<User>>(
                userStoreMock.Object,
                null, null, null, null, null, null, null, null);

            _handler = new ChangeEmailQueryHandler(_mockEmailSender.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ShouldReturnUnauthorized()
        {
            // Arrange
            _mockUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            ChangeEmailQuery command = new(Guid.NewGuid(), Constants.User.Email);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Equal(Error.Unauthorized().Code, result.FirstError.Code);
        }

        [Fact]
        public async Task Handle_EmailAlreadyTaken_ShouldReturnConflict()
        {
            // Arrange
            User user = new()
            { Id = Guid.NewGuid(), Email = Constants.User.Email, Name = Constants.User.Name };
            User existingUser = new()
            { Id = Guid.NewGuid(), Email = Constants.User.Email, Name = Constants.User.Name };

            _mockUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(existingUser);

            ChangeEmailQuery command = new(Guid.NewGuid(), Constants.User.Email);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Equal(Error.Conflict(description: $"Email {Constants.User.Email} already taken").Code, result.FirstError.Code);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnSuccess()
        {
            // Arrange
            User user = new()
            { Id = Guid.NewGuid(), Email = Constants.User.Email, Name = Constants.User.Name };

            _mockUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            _mockEmailSender.Setup(m => m.SendConfirmationEmailAsync(It.IsAny<User>(), It.IsAny<string>(), true))
                .Returns(Task.CompletedTask);

            ChangeEmailQuery command = new(Guid.NewGuid(), "newemail@example.com");

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(Result.Success, result.Value);
        }
    }
}
