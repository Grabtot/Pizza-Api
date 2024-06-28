using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Moq;
using PizzaApi.Application.Users.Commands.SetManager;
using PizzaApi.Domain.Users;
using PizzaApi.Tests.Application.TestUtils.Constants;

namespace PizzaApi.Tests.Application.Account.Commands
{
    public class SetMangerRopeCommandHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly SetMangerRoleCommandHandler _handler;

        public SetMangerRopeCommandHandlerTests()
        {
            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _handler = new SetMangerRoleCommandHandler(_mockUserManager.Object);
        }

        [Fact]
        public async Task HandleSetMangerRopeCommand_UserExists_ShouldReturnSuccess()
        {
            // Arrange
            string email = Constants.User.Email;
            User user = new()
            { Email = email, Name = Constants.User.Name };

            _mockUserManager.Setup(um => um.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.AddToRoleAsync(user, Constants.Authentication.ManagerRole))
                .ReturnsAsync(IdentityResult.Success);

            SetMangerRoleCommand command = new(email);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
        }

        [Fact]
        public async Task HandleSetMangerRopeCommand_UserNotFound_ShouldReturnNotFoundError()
        {
            // Arrange
            string email = Constants.User.Email;

            _mockUserManager.Setup(um => um.FindByEmailAsync(email))
                .ReturnsAsync((User)null);

            SetMangerRoleCommand command = new(email);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Equal(Error.NotFound().Code, result.FirstError.Code);
        }

        [Fact]
        public async Task HandleSetMangerRopeCommand_AddToRoleFailed_ShouldThrowInvalidOperationException()
        {
            // Arrange
            string email = Constants.User.Email;
            User user = new()
            { Email = email, Name = Constants.User.Name };

            _mockUserManager.Setup(um => um.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.AddToRoleAsync(user, Constants.Authentication.ManagerRole))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Role addition failed" }));

            SetMangerRoleCommand command = new(email);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _handler.Handle(command, CancellationToken.None));
        }
    }
}
