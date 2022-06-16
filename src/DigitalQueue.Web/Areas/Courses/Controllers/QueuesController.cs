using System.Security.Claims;

using DigitalQueue.Web.Areas.Courses.Commands;
using DigitalQueue.Web.Areas.Courses.Commands.Queues;
using DigitalQueue.Web.Areas.Courses.Queries;
using DigitalQueue.Web.Filters;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Courses.Controllers
{
    [Route("api/courses/{courseId}/queue")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QueuesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QueuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = nameof(GetQueue))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQueue([FromRoute] string courseId, [FromQuery]bool received)
        {
            return Ok(await _mediator.Send(new GetQueueByCourseIdQuery(courseId, received)));
        }

        [HttpPost("create", Name = nameof(CreateQueueItem))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateQueueItem([FromRoute] string courseId)
        {
            var canCreate = await this._mediator.Send(new CanCreateQueueItemCommand(courseId));
            if (!canCreate)
            {
                return BadRequest(new ErrorDto("You're already the last in the queue."));
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this._mediator.Send(new CreateQueueItemCommand(courseId, currentUserId));
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPatch("{itemId}/complete", Name = nameof(CompleteQueueItem))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CompleteQueueItem([FromRoute] string itemId)
        {
            await this._mediator.Send(new CompleteQueueItemCommand(itemId));
            return NoContent();
        }
    }
}
