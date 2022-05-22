using System.Security.Claims;

using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Areas.Courses.Queries;

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
        [ProducesResponseType(typeof(QueuesDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQueues()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var requests = await this._mediator.Send(new GetCoursesQueuesQuery(currentUserId));
            return Ok(requests);
        }
    }
}
