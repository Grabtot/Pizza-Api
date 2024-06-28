using ErrorOr;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using PizzaApi.Application.Users.Commands.RefreshToken;
using PizzaApi.Domain.Users;
using PizzaApi.Tests.Application.Account.Commands.TestUtils;
using PizzaApi.Tests.Application.TestUtils.Constants;
using System.Security.Claims;

namespace PizzaApi.Tests.Application.Account.Commands
{
    public class RefreshTokenCommandHandlerTests
    {
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly Mock<IOptionsMonitor<BearerTokenOptions>> _mockBearerTokenOptions;
        private readonly Mock<TimeProvider> _mockTimeProvider;
        private readonly RefreshTokenCommandHandler _handler;

        public RefreshTokenCommandHandlerTests()
        {
            Mock<IUserStore<User>> userStoreMock = new();

            Mock<UserManager<User>> mockUserManager = new(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<User>>(
                mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                null, null, null, null);

            _mockBearerTokenOptions = new Mock<IOptionsMonitor<BearerTokenOptions>>();
            _mockTimeProvider = new Mock<TimeProvider>();

            _handler = new RefreshTokenCommandHandler(
                _mockSignInManager.Object,
                _mockBearerTokenOptions.Object,
                _mockTimeProvider.Object);
        }

        [Theory]
        [MemberData(nameof(ValidRefreshTokenCommands))]
        public async Task HandleRefreshTokenCommand_ValidToken_ShouldReturnClaimsPrincipal(RefreshTokenCommand command)
        {
            // Arrange
            AuthenticationProperties properties = new()
            { ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10) };
            AuthenticationTicket refreshTicket = new(
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "userId") })),
                properties, Constants.Authentication.BearerScheme);

            Mock<ISecureDataFormat<AuthenticationTicket>> mockDataFormat = new();
            mockDataFormat.Setup(m => m.Unprotect(command.RefreshToken)).Returns(refreshTicket);

            BearerTokenOptions options = new()
            { RefreshTokenProtector = mockDataFormat.Object };
            _mockBearerTokenOptions.Setup(m => m.Get(Constants.Authentication.BearerScheme)).Returns(options);

            _mockTimeProvider.Setup(tp => tp.GetUtcNow()).Returns(DateTimeOffset.UtcNow);

            User user = new()
            { Id = Guid.NewGuid(), Name = Constants.User.Name };
            _mockSignInManager.Setup(sm => sm.ValidateSecurityStampAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _mockSignInManager.Setup(sm => sm.CreateUserPrincipalAsync(user)).ReturnsAsync(refreshTicket.Principal);

            // Act
            ErrorOr<ClaimsPrincipal> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.NotNull(result.Value);
            Assert.Equal(refreshTicket.Principal, result.Value);
        }

        [Theory]
        [MemberData(nameof(InvalidRefreshTokenCommands))]
        public async Task HandleRefreshTokenCommand_InvalidToken_ShouldReturnUnauthorized(RefreshTokenCommand command)
        {
            // Arrange
            Mock<ISecureDataFormat<AuthenticationTicket>> mockDataFormat = new();
            mockDataFormat.Setup(m => m.Unprotect(command.RefreshToken)).Returns((AuthenticationTicket)null);

            BearerTokenOptions options = new()
            { RefreshTokenProtector = mockDataFormat.Object };
            _mockBearerTokenOptions.Setup(m => m.Get(Constants.Authentication.BearerScheme)).Returns(options);

            _mockTimeProvider.Setup(tp => tp.GetUtcNow()).Returns(DateTimeOffset.UtcNow);

            // Act
            ErrorOr<ClaimsPrincipal> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Equal(Error.Unauthorized().Code, result.Errors.First().Code);
        }

        public static IEnumerable<object[]> ValidRefreshTokenCommands()
        {
            yield return new object[] { CommandsCreateUtils.CreateRefreshTokenCommand() };
        }

        public static IEnumerable<object[]> InvalidRefreshTokenCommands()
        {
            yield return new object[] { CommandsCreateUtils.CreateRefreshTokenCommand("invalidRefreshToken") };
        }
    }
}
