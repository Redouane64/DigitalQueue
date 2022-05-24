using DigitalQueue.Web.Data;
using DigitalQueue.Web.Services.Notifications;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public class CompleteQueueItemCommand : IRequest
{
    public string RequestId { get; }

    public CompleteQueueItemCommand(string requestId)
    {
        RequestId = requestId;
    }
    
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
                    _logger.LogError(e, "Unable to send Firebase notification");
                }
                
                await transaction.CommitAsync(cancellationToken);
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
