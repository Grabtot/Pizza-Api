using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaApi.Api.Models.Allergens;
using PizzaApi.Application.Allergens.Commands.CreateAllergen;
using PizzaApi.Application.Common.Constants;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Api.Controllers
{
    [Route("/api/[controller]")]
    [Authorize(Constants.Account.MangerPolicy)]
    public class AllergensController(IMapper mapper, IMediator mediator) : ApiController(mapper, mediator)
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateAllergenRequest request)
        {
            CreateAllergenCommand command = new(request.Name, request.Description);

            ErrorOr<Allergen> result = await Mediator.Send(command);

            return result.Match(_ => Created(), Problem);
        }
    }
}
