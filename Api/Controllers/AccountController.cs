using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PizzaApi.Api.Models.Users;
using PizzaApi.Application.Common.Constants;
using PizzaApi.Application.Common.Interfaces;
using PizzaApi.Application.Users.Commands.ConfirmAccount;
using PizzaApi.Application.Users.Commands.ConfirmNewEmail;
using PizzaApi.Application.Users.Commands.Login;
using PizzaApi.Application.Users.Commands.RefreshToken;
using PizzaApi.Application.Users.Commands.Register;
using PizzaApi.Application.Users.Commands.ResetPassword;
using PizzaApi.Application.Users.Commands.SetManager;
using PizzaApi.Application.Users.Queries;
using PizzaApi.Application.Users.Queries.ForgotPassword;
using PizzaApi.Application.Users.Queries.ResendConfirmationEmail;
using PizzaApi.Domain.Users;
using System.Security.Claims;

namespace PizzaApi.Api.Controllers
{
    [Route("[controller]")]
    public class AccountController(IMapper mapper,
        IMediator mediator,
        ICurrentUserProvider currentUserProvider) : ApiController(mapper, mediator)
    {
        private readonly ICurrentUserProvider _userProvider = currentUserProvider;

        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> Info()
        {
            Guid id = _userProvider.UserId!.Value;
            ErrorOr<User> result = await Mediator.Send(new UserInfoQuery(id));

            return result.Match(
                user => Ok(Mapper.Map<UserResponse>((user, _userProvider.Role))),
                Problem);
        }

        [HttpPost("createManger")]
        [Authorize(Constants.Account.MangerOrDeveloper)]
        public async Task<IActionResult> CreateManger(string email)
        {
            ErrorOr<Success> result = await Mediator.Send(new SetMangerRopeCommand(email));

            return result.Match(_ => NoContent(), Problem);
        }

        [HttpPost("login")]
        public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login(
            [FromBody] LoginRequest login, [FromQuery] bool? useCookies,
            [FromQuery] bool? useSeccionCookies)
        {
            LoginCommand command = new(login.Email, login.Password, useCookies, useSeccionCookies);

            ErrorOr<Success> result = await Mediator.Send(command);

            if (result.IsError)
            {
                return TypedResults.Problem(result.FirstError.Description,
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            return TypedResults.Empty;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            RegisterCommand command = Mapper.Map<RegisterCommand>(request);

            ErrorOr<User> result = await Mediator.Send(command);

            if (result.IsError)
                return Problem(result.Errors);

            await Mediator.Publish(new UserCreatedEvent(result.Value));

            return Created();
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(Guid userId, string code, string? changedEmail)
        {
            IRequest<ErrorOr<Success>> command = changedEmail is null ?
                new ConfirmAccountCommand(userId, code)
                : new ConfirmNewEmailCommand(userId, code, changedEmail);

            ErrorOr<Success> result = await Mediator.Send(command);

            return result.Match(_ => NoContent(), Problem);
        }

        [HttpPost("refresh")]
        public async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult,
            SignInHttpResult, ChallengeHttpResult>> Refresh(RefreshTokenRequest request)
        {
            ErrorOr<ClaimsPrincipal> result = await Mediator
                .Send(new RefreshTokenCommand(request.RefreshToken));

            if (result.IsError)
                return TypedResults.Challenge();

            return TypedResults.SignIn(result.Value, authenticationScheme: Constants.Account.Bearer);
        }

        [HttpGet("resendConfirmationEmail")]
        public async Task<IActionResult> ResendConfirmationEmail(ResendConfirmationEmailRequest request)
        {
            ErrorOr<Success> result = await Mediator.Send(new ResendConfirmationEmailQuery(request.Email));

            if (result.IsError)
            {
                if (result.FirstError.Type == ErrorType.NotFound)
                    return Ok();

                return Problem(result.Errors);
            }

            return Ok();
        }

        [HttpGet("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            ErrorOr<Success> result = await Mediator.Send(new ForgotPasswordQuery(request.Email));

            return result.Match(_ => NoContent(), Problem);
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            ResetPasswordCommand command = Mapper.Map<ResetPasswordCommand>(request);

            ErrorOr<Success> result = await Mediator.Send(command);

            return result.Match(_ => NoContent(), Problem);
        }

    }
}
