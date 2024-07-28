using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaApi.Api.Models.Allergens;
using PizzaApi.Application.Allergens.Commands.CreateAllergen;
using PizzaApi.Application.Allergens.Commands.DeleteAllergen;
using PizzaApi.Application.Allergens.Commands.UpdateAllergen;
using PizzaApi.Application.Allergens.Queries.GetAllAllergens;
using PizzaApi.Application.Common.Constants;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Api.Controllers
{
    [Route("/api/[controller]")]
    [Authorize(Constants.Account.MangerPolicy)]
    public class AllergensController(IMapper mapper, IMediator mediator) : ApiController(mapper, mediator)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            ErrorOr<List<Allergen>> result
                = await Mediator.Send(new GetAllAllergensQuery());

            return result.Match(allergens
                => Ok(allergens.ConvertAll(Mapper.Map<AllergenResponse>)),
                Problem);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAllergenRequest request)
        {
            CreateAllergenCommand command = new(request.Name, request.Description);

            ErrorOr<Allergen> result = await Mediator.Send(command);

            return result.Match(
                _ => CreatedAtAction(nameof(GetAll), null),
                Problem);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAllergenRequest request)
        {
            UpdateAllergenCommand command = Mapper.Map<UpdateAllergenCommand>(request);

            ErrorOr<Allergen> result = await Mediator.Send(command);

            return result.Match(_ => NoContent(), Problem);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string name)
        {
            ErrorOr<Success> result = await Mediator.Send(new DeleteAllergenCommand(name));

            return result.Match(_ => NoContent(), Problem);
        }
    }
}
