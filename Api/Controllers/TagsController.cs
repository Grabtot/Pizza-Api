using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaApi.Api.Models.Tags;
using PizzaApi.Application.Common.Constants;
using PizzaApi.Application.Tags.Commands.CreateTag;
using PizzaApi.Application.Tags.Commands.DeleteTag;
using PizzaApi.Application.Tags.Commands.UpdateTag;
using PizzaApi.Application.Tags.Queries.GetAllTags;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Api.Controllers
{
    [Route("/api/[controller]")]
    [Authorize(Constants.Account.MangerPolicy)]
    public class TagsController(IMediator mediator, IMapper mapper)
        : ApiController(mapper, mediator)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            List<Tag> tags = await Mediator.Send(new GetAllTagsQuery());

            return Ok(tags.ConvertAll(Mapper.Map<TagResponse>));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagRequest request)
        {
            CreateTagCommand command = Mapper.Map<CreateTagCommand>(request);

            ErrorOr<Tag> result = await Mediator.Send(command);

            return result.Match(_ => CreatedAtAction(nameof(GetAll), null), Problem);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTagRequest request)
        {
            UpdateTagCommand command = Mapper.Map<UpdateTagCommand>(request);

            ErrorOr<Tag> result = await Mediator.Send(command);

            return result.Match(_ => NoContent(), Problem);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string name)
        {
            ErrorOr<Success> result = await Mediator.Send(new DeleteTagCommand(name));

            return result.Match(_ => NoContent(), Problem);
        }
    }
}
