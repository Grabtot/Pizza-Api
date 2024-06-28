using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Moq;
using PizzaApi.Application.Users.Commands.ChangePassword;
using PizzaApi.Domain.Users;
using PizzaApi.Tests.Application.Account.Commands.TestUtils;
using PizzaApi.Tests.Application.TestUtils.Constants;

namespace PizzaApi.Tests.Application.Account.Commands
{
    public class ChangePasswordCommandHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly ChangePasswordCommandHandler _handler;

        public ChangePasswordCommandHandlerTests()
        {
            Mock<IUserStore<User>> userStoreMock = new();

            _mockUserManager = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _handler = new ChangePasswordCommandHandler(_mockUserManager.Object);
        }

        [Theory]
        [MemberData(nameof(ValidChangePasswordCommands))]
        public async Task HandleChangePasswordCommand_UserIsValid_ShouldReturnSuccess(ChangePasswordCommand command)
        {
            // Arrange
            User user = new()
            { Id = command.UserId, Name = Constants.User.Name };

            _mockUserManager.Setup(um => um.FindByIdAsync(command.UserId.ToString()))
                .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.ChangePasswordAsync(user, command.OldPassword, command.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(Result.Success, result.Value);
        }

        [Theory]
        [MemberData(nameof(InvalidChangePasswordCommands))]
        public async Task HandleChangePasswordCommand_UserIsInvalid_ShouldThrowException(ChangePasswordCommand command)
        {
            // Arrange
            _mockUserManager.Setup(um => um.FindByIdAsync(command.UserId.ToString()))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [MemberData(nameof(InvalidChangePasswordCommands))]
        public async Task HandleChangePasswordCommand_InvalidPassword_ShouldReturnErrors(ChangePasswordCommand command)
        {
            // Arrange
            User user = new()
            { Id = command.UserId, Name = Constants.User.Name };

            List<IdentityError> identityErrors = new()
            {
                new IdentityError { Code = "InvalidPassword", Description = "Invalid old password." }
            };

            IdentityResult identityResult = IdentityResult.Failed(identityErrors.ToArray());

            _mockUserManager.Setup(um => um.FindByIdAsync(command.UserId.ToString()))
                .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.ChangePasswordAsync(user, command.OldPassword, command.NewPassword))
                .ReturnsAsync(identityResult);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal("InvalidPassword", result.Errors.First().Code);
            Assert.Equal("Invalid old password.", result.Errors.First().Description);
        }

        public static IEnumerable<object[]> ValidChangePasswordCommands()
        {
            yield return new object[] { CommandsCreateUtils.CreateChangePasswordCommand() };
            yield return new object[] { CommandsCreateUtils.CreateChangePasswordCommand(
                userId: Guid.NewGuid(),
                oldPassword: "OldStrongPass123!",
                newPassword: "NewStrongPass123!") };
        }

        public static IEnumerable<object[]> InvalidChangePasswordCommands()
        {
            yield return new object[] { CommandsCreateUtils.CreateChangePasswordCommand(
                userId: Guid.NewGuid(),
                oldPassword: "WrongOldPass123!",
                newPassword: "WeakPass") };
        }
    }
}
