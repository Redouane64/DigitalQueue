using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using DigitalQueue.Web.Areas.Courses.Commands;
using DigitalQueue.Web.Areas.Courses.Queries;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GetCourses()
        {
            var courses = await this._mediator.Send(new GetCoursesQuery());
            return Ok(courses);
        }

        [HttpGet("get-requests-queue", Name = nameof(GetRequestsQueue))]
        public async Task<IActionResult> GetRequestsQueue()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var requests = await this._mediator.Send(new GetCourseRequestsQuery(currentUserId));
            return Ok(requests);
        }
        
        [HttpPost("create-request", Name = nameof(CreateRequest))]
        public async Task<IActionResult> CreateRequest([FromQuery] string courseId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this._mediator.Send(new CreateCourseRequestCommand(courseId, currentUserId));
            return Ok();
        }

        [HttpPost("complete-request", Name= nameof(CompleteRequest))]
        public async Task<IActionResult> CompleteRequest([FromQuery] string courseId, [FromQuery] string requestId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this._mediator.Send(new CompleteCourseRequestCommand(requestId, courseId));
            return Ok();
        }
    }
}
