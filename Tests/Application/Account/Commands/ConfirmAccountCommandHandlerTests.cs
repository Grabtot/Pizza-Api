using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using PizzaApi.Application.Users.Commands.ConfirmAccount;
using PizzaApi.Domain.Users;
using PizzaApi.Tests.Application.Account.Commands.TestUtils;
using PizzaApi.Tests.Application.TestUtils.Constants;
using System.Text;

namespace PizzaApi.Tests.Application.Account.Commands
{
    public class ConfirmAccountCommandHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly ConfirmAccountCommandHandler _handler;

        public ConfirmAccountCommandHandlerTests()
        {
            Mock<IUserStore<User>> userStoreMock = new();

            _mockUserManager = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _handler = new ConfirmAccountCommandHandler(_mockUserManager.Object);
        }

        [Theory]
        [MemberData(nameof(ValidConfirmAccountCommands))]
        public async Task HandleConfirmAccountCommand_UserIsValid_ShouldReturnSuccess(ConfirmAccountCommand command)
        {
            // Arrange
            User user = new()
            {
                Id = command.UserId,
                Email = Constants.User.Email,
                Name = Constants.User.Name
            };
            string decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.Code));

            _mockUserManager.Setup(um => um.FindByIdAsync(command.UserId.ToString()))
                .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.ConfirmEmailAsync(user, decodedCode))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(Result.Success, result.Value);
        }

        [Theory]
        [MemberData(nameof(InvalidConfirmAccountCommands))]
        public async Task HandleConfirmAccountCommand_UserIsInvalid_ShouldReturnUnauthorized(ConfirmAccountCommand command)
        {
            // Arrange
            _mockUserManager.Setup(um => um.FindByIdAsync(command.UserId.ToString()))
                .ReturnsAsync((User)null);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains(result.Errors, e => e.Code == Error.Unauthorized().Code);
        }

        [Theory]
        [MemberData(nameof(InvalidConfirmAccountCommands))]
        public async Task HandleConfirmAccountCommand_InvalidCode_ShouldReturnErrors(ConfirmAccountCommand command)
        {
            // Arrange
            User user = new()
            {
                Id = command.UserId,
                Email = Constants.User.Email,
                Name = Constants.User.Name
            };
            string decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.Code));

            List<IdentityError> identityErrors = new()
            {
                new IdentityError { Code = "InvalidToken", Description = "Invalid confirmation token." }
            };

            IdentityResult identityResult = IdentityResult.Failed(identityErrors.ToArray());

            _mockUserManager.Setup(um => um.FindByIdAsync(command.UserId.ToString()))
                .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.ConfirmEmailAsync(user, decodedCode))
                .ReturnsAsync(identityResult);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal("InvalidToken", result.Errors.First().Code);
            Assert.Equal("Invalid confirmation token.", result.Errors.First().Description);
        }

        public static IEnumerable<object[]> ValidConfirmAccountCommands()
        {
            yield return new object[] { CommandsCreateUtils.CreateConfirmAccountCommand() };
            yield return new object[] { CommandsCreateUtils.CreateConfirmAccountCommand(
                userId: Guid.NewGuid(),
                code: WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("validCode"))) };
        }

        public static IEnumerable<object[]> InvalidConfirmAccountCommands()
        {
            yield return new object[] { CommandsCreateUtils.CreateConfirmAccountCommand(
                userId: Guid.NewGuid(),
                code: WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("invalidCode"))) };
        }
    }
}
