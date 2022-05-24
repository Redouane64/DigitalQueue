using System.Security.Claims;

using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public class CanCreateQueueItemCommand : IRequest<bool>
{
    public string CourseId { get; }

    public CanCreateQueueItemCommand(string courseId)
    {
        CourseId = courseId;
    }

    public class CanCreateQueueItemCommandHandler : IRequestHandler<CanCreateQueueItemCommand, bool>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DigitalQueueContext _context;
        private readonly ILogger<CanCreateQueueItemCommandHandler> _logger;

        public CanCreateQueueItemCommandHandler(
            IHttpContextAccessor httpContextAccessor,
            DigitalQueueContext context,
            ILogger<CanCreateQueueItemCommandHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(CanCreateQueueItemCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var lastQueueItem = await _context.Queues
                .OrderByDescending(e => e.CreateAt)
                .FirstOrDefaultAsync(e => e.CreatorId == currentUserId && e.CourseId == request.CourseId, cancellationToken);

            return lastQueueItem is null;
        }
    }
}
