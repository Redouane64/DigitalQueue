using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public class CreateCourseRequestCommand : IRequest
{
    public string CourseId { get; }
    public string UserId { get; }

    public CreateCourseRequestCommand(string courseId, string userId)
    {
        CourseId = courseId;
        UserId = userId;
    }
    
    public class CreateCourseRequestCommandHandler : IRequestHandler<CreateCourseRequestCommand>
    {
        private readonly DigitalQueueContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CreateCourseRequestCommandHandler> _logger;

        public CreateCourseRequestCommandHandler(DigitalQueueContext context, UserManager<User> userManager, ILogger<CreateCourseRequestCommandHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(CreateCourseRequestCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await this._context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var course =
                    await this._context.Courses.FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken);

                if (course is null)
                {
                    return Unit.Value;
                }

                var user = await _userManager.FindByIdAsync(request.UserId);

                if (user is null)
                {
                    return Unit.Value;
                }

                var courseRequest = new QueueItem { Id = Guid.NewGuid().ToString(), Course = course, Creator = user };
                _context.Add(courseRequest);

                await _context.SaveChangesAsync(cancellationToken);
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypesDefaults.Student, course.Id));
                
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(
                    e, "Unable to create request for course {} with user {}", 
                    request.CourseId, request.UserId);
            }

            return Unit.Value;
        }
    }
}
