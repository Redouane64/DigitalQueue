using DigitalQueue.Web.Data;

using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public partial class ArchiveCourseCommand
{
    public class ArchiveCourseCommandHandler : IRequestHandler<ArchiveCourseCommand>
    {
        private readonly DigitalQueueContext _context;
        private readonly ILogger<ArchiveCourseCommandHandler> _logger;

        public ArchiveCourseCommandHandler(DigitalQueueContext context, ILogger<ArchiveCourseCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(ArchiveCourseCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var course = await _context.Courses.FindAsync(new String[] { request.CourseId });
                if (course is null)
                {
                    return Unit.Value;
                }

                course.IsArchived = true;

                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to archive course {CourseId}", request.CourseId);
                await transaction.RollbackAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
