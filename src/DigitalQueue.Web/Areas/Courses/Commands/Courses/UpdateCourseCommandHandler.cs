using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Commands.Courses;

public class UpdateCourseCommand : IRequest<bool>
{
    public string Id { get; }
    public string? Title { get; }
    public int Year { get; }

    public UpdateCourseCommand(string id, string? title, int year)
    {
        Id = id;
        Title = title;
        Year = year;
    }
}

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, bool>
{
    private readonly DigitalQueueContext _context;
    private readonly ILogger<UpdateCourseCommandHandler> _logger;

    public UpdateCourseCommandHandler(DigitalQueueContext context, ILogger<UpdateCourseCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await this._context.Courses
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course is null)
        {
            _logger.LogWarning("Course with id {Id} is not found", request.Id);
            return false;
        }

        course.Name = request.Title;
        course.Year = request.Year;

        try
        {
            _context.Update(course);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to update course");
            return false;
        }

        return true;
    }
}

