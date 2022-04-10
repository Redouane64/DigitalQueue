using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public class CompleteCourseRequestCommand : IRequest
{
    public string RequestId { get; }
    public string UserId { get; }

    public CompleteCourseRequestCommand(string requestId, string userId)
    {
        RequestId = requestId;
        UserId = userId;
    }
    
    public class CompleteCourseRequestCommandHandler : IRequestHandler<CompleteCourseRequestCommand>
    {
        private readonly DigitalQueueContext _context;
        private readonly ILogger<CompleteCourseRequestCommandHandler> _logger;

        public CompleteCourseRequestCommandHandler(DigitalQueueContext context, ILogger<CompleteCourseRequestCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(CompleteCourseRequestCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var courseRequest =
                    await _context.Requests.FirstOrDefaultAsync(r =>
                        r.Id == request.RequestId && r.CreatorId == request.UserId, cancellationToken: cancellationToken);

                if (courseRequest is null)
                {
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
                    e, "Unable to modify request {}", 
                    request.RequestId);
            }
            
            return Unit.Value;
        }
    }
}
