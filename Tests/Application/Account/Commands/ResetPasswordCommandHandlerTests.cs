using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using PizzaApi.Application.Users.Commands.ResetPassword;
using PizzaApi.Domain.Users;
using PizzaApi.Tests.Application.Account.Commands.TestUtils;
using PizzaApi.Tests.Application.TestUtils.Constants;
using System.Text;

namespace PizzaApi.Tests.Application.Account.Commands
{
    public class ResetPasswordCommandHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly ResetPasswordCommandHandler _handler;

        public ResetPasswordCommandHandlerTests()
        {
            Mock<IUserStore<User>> userStoreMock = new();

            _mockUserManager = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _handler = new ResetPasswordCommandHandler(_mockUserManager.Object);
        }

        [Theory]
        [MemberData(nameof(ValidResetPasswordCommands))]
        public async Task HandleResetPasswordCommand_UserIsValid_ShouldReturnSuccess(ResetPasswordCommand command)
        {
            // Arrange
            User user = new()
            {
                Email = command.Email,
                Name = Constants.User.Name,
                EmailConfirmed = true
            };
            string decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.ResetCode));

            _mockUserManager.Setup(um => um.FindByEmailAsync(command.Email))
                .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.ResetPasswordAsync(user, decodedCode, command.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(Result.Success, result.Value);
        }

        [Theory]
        [MemberData(nameof(InvalidResetPasswordCommands))]
        public async Task HandleResetPasswordCommand_UserIsInvalid_ShouldReturnUnauthorized(ResetPasswordCommand command)
        {
            // Arrange
            _mockUserManager.Setup(um => um.FindByEmailAsync(command.Email))
                .ReturnsAsync((User)null);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains(result.Errors, e => e.Code == Error.Unauthorized().Code);
        }

        [Theory]
        [MemberData(nameof(InvalidResetPasswordCommands))]
        public async Task HandleResetPasswordCommand_InvalidCode_ShouldReturnErrors(ResetPasswordCommand command)
        {
            // Arrange
            User user = new()
            { Email = command.Email, Name = Constants.User.Name, EmailConfirmed = true };
            string decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.ResetCode));

            List<IdentityError> identityErrors = new()
            {
                new IdentityError { Code = "InvalidToken", Description = "Invalid reset token." }
            };

            IdentityResult identityResult = IdentityResult.Failed(identityErrors.ToArray());

            _mockUserManager.Setup(um => um.FindByEmailAsync(command.Email))
                .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.ResetPasswordAsync(user, decodedCode, command.NewPassword))
                .ReturnsAsync(identityResult);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal("InvalidToken", result.Errors.First().Code);
            Assert.Equal("Invalid reset token.", result.Errors.First().Description);
        }

        public static IEnumerable<object[]> ValidResetPasswordCommands()
        {
            yield return new object[] { CommandsCreateUtils.CreateResetPasswordCommand() };
            yield return new object[] { CommandsCreateUtils.CreateResetPasswordCommand(
                email: "test2@example.com",
                resetCode: WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("validResetCode2")),
                newPassword: "NewStrongPass123!") };
        }

        public static IEnumerable<object[]> InvalidResetPasswordCommands()
        {
            yield return new object[] { CommandsCreateUtils.CreateResetPasswordCommand(
                email: "invalid@example.com",
                resetCode: WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("invalidResetCode")),
                newPassword: "WeakPass") };
        }
    }
}
