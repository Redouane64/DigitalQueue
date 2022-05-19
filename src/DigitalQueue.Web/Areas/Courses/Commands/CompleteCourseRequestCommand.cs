using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public class CompleteCourseRequestCommand : IRequest
{
    public string RequestId { get; }

    public CompleteCourseRequestCommand(string requestId)
    {
        RequestId = requestId;
    }
    
    public class CompleteCourseRequestCommandHandler : IRequestHandler<CompleteCourseRequestCommand>
    {
        private readonly DigitalQueueContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CompleteCourseRequestCommandHandler> _logger;

        public CompleteCourseRequestCommandHandler(
            DigitalQueueContext context, 
            IHttpContextAccessor httpContextAccessor,
            ILogger<CompleteCourseRequestCommandHandler> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(CompleteCourseRequestCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var courseRequest = await _context.Queues.FirstOrDefaultAsync(
                    r => r.Id == request.RequestId,
                    cancellationToken: cancellationToken
                );

                if (courseRequest is null)
                {
                    return Unit.Value;
                }

                var teacherOf = _httpContextAccessor.HttpContext.User.FindAll(ClaimTypesDefaults.Teacher);
                if (!teacherOf.Any(c => c.Value == courseRequest.CourseId))
                {
                    _logger.LogWarning("Completion of queue item only allowed from course teacher");
                    return Unit.Value;
                }
                
                courseRequest.Completed = true;
                await _context.SaveChangesAsync(cancellationToken);
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
