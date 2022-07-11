using System.Security.Claims;

using DigitalQueue.Web.Areas.Notifications.Models;
using DigitalQueue.Web.Areas.Notifications.Services;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Common;
using DigitalQueue.Web.Data.Courses;
using DigitalQueue.Web.Data.Users;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Commands.Queues;

public class CreateQueueItemCommand : IRequest
{
    public string CourseId { get; }
    public string UserId { get; }

    public CreateQueueItemCommand(string courseId, string userId)
    {
        CourseId = courseId;
        UserId = userId;
    }
}

public class CreateQueueItemCommandHandler : IRequestHandler<CreateQueueItemCommand>
{
    private readonly DigitalQueueContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly FirebaseService _firebaseService;
    private readonly ILogger<CreateQueueItemCommandHandler> _logger;

    public CreateQueueItemCommandHandler(
        DigitalQueueContext context,
        UserManager<ApplicationUser> userManager,
        FirebaseService firebaseService,
        ILogger<CreateQueueItemCommandHandler> logger)
    {
        _context = context;
        _userManager = userManager;
        _firebaseService = firebaseService;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateQueueItemCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await this._context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var course =
                await this._context.Courses
                    .Include(c => c.Teachers)
                    .FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken);

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

            await _userManager.AddClaimAsync(user, new Claim(ClaimTypesDefaults.Student, course.Id));

            // send firebase notification
            try
            {
                var teacherIds = course.Teachers.Select(t => t.Id).ToArray();
                var tokens = await _context.Sessions
                    .Where(s => teacherIds.Contains(s.UserId))
                    .Select(s => s.DeviceToken)
                    .ToArrayAsync(cancellationToken);
                await _firebaseService.Send(new FirebaseNotification(
                    tokens,
                    subject: $"New student waiting in {course.Name} queue",
                    body: "Student added to queue"));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to send Firebase notification");
            }

            await _context.SaveChangesAsync(cancellationToken);
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
