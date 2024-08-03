using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaApi.Api.Models.Ingredients;
using PizzaApi.Application.Common.Constants;
using PizzaApi.Application.Ingredients.Commands.CreateIngredient;
using PizzaApi.Application.Ingredients.Queriers.GetDetails;
using PizzaApi.Domain.Ingredients;

namespace PizzaApi.Api.Controllers
{
    [Route("/api/[controller]")]
    [Authorize(Constants.Account.MangerPolicy)]
    public class IngredientsController(IMapper mapper, IMediator mediator)
        : ApiController(mapper, mediator)
    {
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            ErrorOr<Ingredient> result = await Mediator.Send(new GetIngredientDetailsQuery(id));

            return result.Match(
                ingredient => Ok(Mapper.Map<IngredientDetailsResponse>(ingredient)),
                Problem);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateIngredientRequest request)
        {
            CreateIngredientCommand command = Mapper.Map<CreateIngredientCommand>(request);

            ErrorOr<Ingredient> result = await Mediator.Send(command);

            return result.Match(
                ingredient => Created(nameof(Details), Mapper.Map<IngredientDetailsResponse>(ingredient)),
                Problem);
        }
    }
}
