using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using PizzaApi.Application.Users.Commands.Login;
using PizzaApi.Domain.Users;
using PizzaApi.Tests.Application.Account.Commands.TestUtils;
using PizzaApi.Tests.Application.TestUtils.Constants;

namespace PizzaApi.Tests.Application.Account.Commands
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            Mock<IUserStore<User>> userStoreMock = new();

            _mockUserManager = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<User>>(
                _mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                null, null, null, null);

            _handler = new LoginCommandHandler(_mockSignInManager.Object);
        }

        [Theory]
        [MemberData(nameof(ValidLoginCommands))]
        public async Task HandleLoginCommand_UserIsValid_ShouldReturnSuccess(LoginCommand command)
        {
            // Arrange
            User user = new()
            { Email = command.Email, Name = Constants.User.Name };
            _mockUserManager.Setup(um => um.FindByEmailAsync(command.Email))
                .ReturnsAsync(user);

            _mockSignInManager.Setup(sm => sm.PasswordSignInAsync(user, command.Password, It.IsAny<bool>(), false))
                .ReturnsAsync(SignInResult.Success);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(Result.Success, result.Value);
        }

        [Theory]
        [MemberData(nameof(InvalidLoginCommands))]
        public async Task HandleLoginCommand_UserIsInvalid_ShouldReturnUnauthorized(LoginCommand command)
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
        [MemberData(nameof(InvalidLoginCommands))]
        public async Task HandleLoginCommand_InvalidPassword_ShouldReturnUnauthorized(LoginCommand command)
        {
            // Arrange
            User user = new()
            { Email = command.Email, Name = Constants.User.Name };
            _mockUserManager.Setup(um => um.FindByEmailAsync(command.Email))
                .ReturnsAsync(user);

            _mockSignInManager.Setup(sm => sm.PasswordSignInAsync(user, command.Password, It.IsAny<bool>(), false))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains(result.Errors, e => e.Code == Error.Unauthorized().Code);
        }

        public static IEnumerable<object[]> ValidLoginCommands()
        {
            yield return new object[] { CommandsCreateUtils.CreateLoginCommand() };
            yield return new object[] { CommandsCreateUtils.CreateLoginCommand(
                email: "bob@example.com",
                password: "AnotherValidPassword!",
                useCookies: false,
                useSessionCookies: true) };
        }

        public static IEnumerable<object[]> InvalidLoginCommands()
        {
            yield return new object[] { CommandsCreateUtils.CreateLoginCommand(
                email: "nonexistent@example.com",
                password: "Password123!") };
            yield return new object[] { CommandsCreateUtils.CreateLoginCommand(
                email: "test@example.com",
                password: "InvalidPassword!") };
        }
    }
}
