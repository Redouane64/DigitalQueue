using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public partial class SetTeacherCommand
{
    public class SetTeacherCommandHandler : IRequestHandler<SetTeacherCommand>
    {
        private readonly DigitalQueueContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<SetTeacherCommandHandler> _logger;

        public SetTeacherCommandHandler(DigitalQueueContext context, UserManager<User> userManager, ILogger<SetTeacherCommandHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Unit> Handle(SetTeacherCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var course = await _context.Courses
                    .Include(c => c.Teachers)
                    .FirstOrDefaultAsync(c => c.Id.Equals(request.CourseId), cancellationToken: cancellationToken);

                if (course is null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Unit.Value;
                }

                var teachers = await _userManager.Users
                    .Where(u => request.Teachers.Contains(u.Id))
                    .ToArrayAsync(cancellationToken);

                course.Teachers = course.Teachers.UnionBy(teachers, u => u.Id).ToArray();

                _context.Entry(course).Collection(e => e.Teachers).IsModified = true;
                foreach (var teacher in teachers)
                {
                    await _userManager.AddClaimAsync(teacher, new Claim(ClaimTypesDefaults.Teacher, course.Id));
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to add teachers");
                await transaction.RollbackAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
