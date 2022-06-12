using DigitalQueue.Web.Data;
using DigitalQueue.Web.Services.Notifications;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public partial class CompleteQueueItemCommand
{
    public class CompleteQueueItemCommandHandler : IRequestHandler<CompleteQueueItemCommand>
    {
        private readonly DigitalQueueContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FirebaseNotificationService _firebaseNotificationService;
        private readonly ILogger<CompleteQueueItemCommandHandler> _logger;

        public CompleteQueueItemCommandHandler(
            DigitalQueueContext context,
            IHttpContextAccessor httpContextAccessor,
            FirebaseNotificationService firebaseNotificationService,
            ILogger<CompleteQueueItemCommandHandler> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _firebaseNotificationService = firebaseNotificationService;
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

                    await _firebaseNotificationService.Send(
                        new FirebaseNotification(userDevicesTokens,
                            "Queue item completed",
                            $"{queueItem.Course.Title} queue item has been marked complete by course teacher"
                        )
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

                    await _firebaseNotificationService.SendData(
                        userDevicesTokens,
                        new Dictionary<string, string>()
                        {
                            ["type"] = "update",
                            ["course"] = queueItem.CourseId,
                        }
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

                        await _firebaseNotificationService.Send(
                            new FirebaseNotification(currentStudentDeviceTokens,
                                $"Course {currentStudent.Course.Title} queue",
                                $"You are current in {currentStudent.Course.Title} queue"));
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
}
