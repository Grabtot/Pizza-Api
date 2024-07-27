using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaApi.Application.Common.Constants;

namespace PizzaApi.Api.Controllers
{
    [Route("/api/[controller]")]
    [Authorize(Constants.Account.MangerPolicy)]
    public class IngredientController(IMapper mapper, IMediator mediator) : ApiController(mapper, mediator)
    {
        //[HttpPost("")]
    }
}
