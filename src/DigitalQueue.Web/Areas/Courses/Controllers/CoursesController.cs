using System.Security.Claims;

using DigitalQueue.Web.Areas.Courses.Commands;
using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Areas.Courses.Queries;
using DigitalQueue.Web.Filters;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Courses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = nameof(GetCourses))]
        [Produces(typeof(IEnumerable<CourseDto>))]
        public async Task<IActionResult> GetCourses([FromQuery]string q)
        {
            var courses = await this._mediator.Send(new GetCoursesQuery(q));
            return Ok(courses);
        }

        [HttpGet("get-queues", Name = nameof(GetQueues))]
        [Produces(typeof(QueueDto))]
        public async Task<IActionResult> GetQueues()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var requests = await this._mediator.Send(new GetCoursesQueuesQuery(currentUserId));
            return Ok(requests);
        }
        
        [HttpPost("create-queue-item", Name = nameof(CreateQueueItem))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateQueueItem([FromQuery] string courseId)
        {
            var canCreate = await this._mediator.Send(new CanCreateCourseRequestCommand(courseId));
            if (!canCreate) 
            {
                return BadRequest(new ErrorDto("You already the last in the queue."));
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this._mediator.Send(new CreateCourseRequestCommand(courseId, currentUserId));
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("complete-queue-item", Name= nameof(CompleteQueueItem))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CompleteQueueItem([FromQuery] string itemId)
        {
            await this._mediator.Send(new CompleteCourseRequestCommand(itemId));
            return NoContent();
        }
    }
}
