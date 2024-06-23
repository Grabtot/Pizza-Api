using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PizzaApi.Application.Common.Constants;
using PizzaApi.Domain.Users;
using System.Security.Claims;

namespace PizzaApi.Application.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler(
        SignInManager<User> signInManager,
        IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
        TimeProvider timeProvider)
            : IRequestHandler<RefreshTokenCommand, ErrorOr<ClaimsPrincipal>>
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly IOptionsMonitor<BearerTokenOptions> _bearerTokenOptions = bearerTokenOptions;
        private readonly TimeProvider _timeProvider = timeProvider;

        public async Task<ErrorOr<ClaimsPrincipal>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            ISecureDataFormat<AuthenticationTicket> refreshTokenProtector = _bearerTokenOptions
                .Get(Constants.Account.Bearer).RefreshTokenProtector;
            AuthenticationTicket? refreshTicket = refreshTokenProtector.Unprotect(command.RefreshToken);

            if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
                _timeProvider.GetUtcNow() >= expiresUtc ||
                await _signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not User user)

            {
                return Error.Unauthorized();
            }

            return await _signInManager.CreateUserPrincipalAsync(user);
        }
    }
}
