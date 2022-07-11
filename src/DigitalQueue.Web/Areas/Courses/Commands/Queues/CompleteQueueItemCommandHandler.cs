using DigitalQueue.Web.Areas.Notifications.Models;
using DigitalQueue.Web.Areas.Notifications.Services;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Common;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Commands.Queues;

public partial class CanCreateQueueItemCommand : IRequest<bool>
{
    public string CourseId { get; }

    public CanCreateQueueItemCommand(string courseId)
    {
        CourseId = courseId;
    }
}

public class CompleteQueueItemCommandHandler : IRequestHandler<CompleteQueueItemCommand>
{
    private readonly DigitalQueueContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly FirebaseService _firebaseService;
    private readonly ILogger<CompleteQueueItemCommandHandler> _logger;

    public CompleteQueueItemCommandHandler(
        DigitalQueueContext context,
        IHttpContextAccessor httpContextAccessor,
        FirebaseService firebaseService,
        ILogger<CompleteQueueItemCommandHandler> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _firebaseService = firebaseService;
        _logger = logger;
    }

    public async Task<Unit> Handle(CompleteQueueItemCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var queueItem = await _context.Queues
                .Include(e => e.Course)
                .FirstOrDefaultAsync(
                    r => r.Id == request.RequestId,
                    cancellationToken: cancellationToken
                );

            if (queueItem is null)
            {
                return Unit.Value;
            }

            var teacherOf = _httpContextAccessor.HttpContext.User.FindAll(ClaimTypesDefaults.Teacher);
            if (!teacherOf.Any(c => c.Value == queueItem.CourseId))
            {
                _logger.LogWarning("Completion of queue item only allowed from course teacher");
                return Unit.Value;
            }

            queueItem.Completed = true;
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            // send firebase push notification
            try
            {
                var userDevicesTokens = await _context.Sessions
                    .AsNoTracking()
                    .Where(s => s.UserId == queueItem.CreatorId)
                    .Select(s => s.DeviceToken)
                    .ToArrayAsync(cancellationToken);

                await _firebaseService.Send(
                    new FirebaseNotification(userDevicesTokens,
                        subject: $"{queueItem.Course.Name} queue item has been marked complete by course teacher", 
                        body: "Queue item completed")
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to send notification");
            }

            // send update
            try
            {
                var userDevicesTokens = await _context.Sessions
                    .AsNoTracking()
                    .Where(s => s.UserId == queueItem.CreatorId)
                    .Select(s => s.DeviceToken)
                    .ToArrayAsync(cancellationToken);

                await _firebaseService.Send(
                    new FirebaseNotification(
                        userDevicesTokens,
                        new Dictionary<string, string>
                        {
                            ["type"] = "update",
                            ["course"] = queueItem.CourseId,
                        })
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to send update notification");
            }
            // notify current student
            try
            {
                var currentStudent =
                    await _context.Queues.OrderBy(q => q.CreateAt)
                        .FirstOrDefaultAsync(cancellationToken);
                
                if (currentStudent is not null)
                {
                    var currentStudentDeviceTokens = await _context.Sessions
                        .Where(s => s.UserId == currentStudent.CreatorId && !String.IsNullOrEmpty(s.DeviceToken))
                        .Select(s => s.DeviceToken)
                        .ToArrayAsync(cancellationToken);

                    await _firebaseService.Send(
                        new FirebaseNotification(currentStudentDeviceTokens,
                            subject: $"You are current in {currentStudent.Course.Name} queue", 
                            body: $"Course {currentStudent.Course.Name} queue"));
                }
            }
            catch (Exception exception) 
            {
                _logger.LogError(exception, "Unable to send update notification");
            }

        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(
                e, "Unable to update queue item {RequestId}",
                request.RequestId);
        }

        return Unit.Value;
    }
}

