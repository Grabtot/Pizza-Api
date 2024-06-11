using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaApi.Api.Models.Users;
using PizzaApi.Application.Common.Interfaces;
using PizzaApi.Application.Users.Commands;
using PizzaApi.Application.Users.Queries;
using PizzaApi.Domain.Users;

namespace PizzaApi.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AccountController(IMapper mapper,
        IMediator mediator,
        ICurrentUserProvider currentUserProvider) : ApiController(mapper, mediator)
    {
        private readonly ICurrentUserProvider _userProvider = currentUserProvider;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Guid id = _userProvider.UserId!.Value;
            ErrorOr<User> result = await Mediator.Send(new GetUserQuery(id));

            return result.Match(
                user => Ok(Mapper.Map<UserResponse>(user)),
                Problem);
        }

        [HttpPost("createManger")]
        //[Authorize(Constants.Policies.MangerOrDeveloper)]
        public async Task<IActionResult> CreateManger([FromQuery] Guid userId)
        {
            ErrorOr<Success> result = await Mediator.Send(new SetMangerRopeCommand(userId));

            return result.Match(_ => NoContent(), Problem);
        }
    }
}
