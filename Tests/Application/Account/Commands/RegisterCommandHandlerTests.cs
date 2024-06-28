using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Moq;
using PizzaApi.Application.Users.Commands.Register;
using PizzaApi.Domain.Users;
using PizzaApi.Tests.Application.Account.Commands.TestUtils;

namespace PizzaApi.Tests.Application.Account.Commands
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null!, null!, null!, null!, null!, null!, null!, null!);
            _handler = new(_mockUserManager.Object);
        }

        [Theory]
        [MemberData(nameof(ValidRegisterCommands))]
        public async Task HandleRegisterCommand_UserIsValid_ShouldReturnUser(RegisterCommand command)
        {
            // Arrange
            IdentityResult identityResult = IdentityResult.Success;
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), command.Password))
                .ReturnsAsync(identityResult);

            // Act
            ErrorOr<User> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.NotNull(result.Value);
            Assert.Equal(command.Name, result.Value.Name);
            Assert.Equal(command.Email, result.Value.UserName);
            Assert.Equal(command.Email, result.Value.Email);
        }

        public static IEnumerable<object[]> ValidRegisterCommands()
        {
            yield return [CommandsCreateUtils.CreateRegisterCommand()];

            yield return [CommandsCreateUtils.CreateRegisterCommand(
                name: "bob",
                password: "TestPass1234!")];
        }

        [Theory]
        [MemberData(nameof(InvalidRegisterCommands))]
        public async Task HandleRegisterCommand_UserIsInvalid_ShouldReturnErrors(RegisterCommand command)
        {
            // Arrange
            List<IdentityError> identityErrors = new()
            {
        new IdentityError { Code = "DuplicateEmail", Description = "Email is already taken." }
    };
            IdentityResult identityResult = IdentityResult.Failed(identityErrors.ToArray());
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), command.Password))
                .ReturnsAsync(identityResult);

            // Act
            ErrorOr<User> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal("DuplicateEmail", result.Errors.First().Code);
            Assert.Equal("Email is already taken.", result.Errors.First().Description);
        }

        public static IEnumerable<object[]> InvalidRegisterCommands()
        {
            yield return new object[] { CommandsCreateUtils.CreateRegisterCommand(
        name: "alice",
        email: "alice@example.com",
        password: "WeakPass") };
        }

    }
}
