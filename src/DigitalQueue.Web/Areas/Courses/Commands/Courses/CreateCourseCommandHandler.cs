using System.Security.Claims;

using DigitalQueue.Web.Areas.Notifications.Models;
using DigitalQueue.Web.Areas.Notifications.Services;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Common;
using DigitalQueue.Web.Data.Courses;
using DigitalQueue.Web.Data.Users;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Commands.Courses;


public partial class CreateCourseCommand : IRequest<Course?>
{
    public CreateCourseCommand(string title, string[] teachers, int? year = null)
    {
        Title = title;
        Teachers = teachers;
        Year = year ?? DateTime.Now.Year;
    }

    [Required]
    public string Title { get; }

    [Required]
    public string[]? Teachers { get; }

    public int Year { get; }
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Course?>
{
    private readonly DigitalQueueContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly FirebaseService _firebaseService;
    private readonly ILogger<CreateCourseCommandHandler> _logger;

    public CreateCourseCommandHandler(
        DigitalQueueContext context,
        UserManager<ApplicationUser> userManager,
        FirebaseService firebaseService,
        ILogger<CreateCourseCommandHandler> logger)
    {
        _context = context;
        _userManager = userManager;
        _firebaseService = firebaseService;
        _logger = logger;
    }

    public async Task<Course?> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        if (request.Teachers is null || request.Teachers.Length == 0)
        {
            return null;
        }

        await using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                var teachers = await _userManager.Users
                    .Where(u => request.Teachers.Contains(u.Id))
                    .ToArrayAsync(cancellationToken);

                var course = new Course
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Title,
                    Teachers = teachers,
                    Year = request.Year
                };

                await _context.AddAsync(course, cancellationToken);

                foreach (var teacher in teachers)
                {
                    var result = await _userManager.AddClaimAsync(
                        teacher,
                        new Claim(
                            ClaimTypesDefaults.Teacher,
                            course.Id
                        )
                    );
                    if (!result.Succeeded)
                    {
                        await transaction.RollbackAsync(cancellationToken);

                        return null;
                    }
                }

                // send firebase notifications to teachers
                try
                {
                    var teacherIds = teachers.Select(t => t.Id).ToArray();
                    var tokens = await _context.Sessions
                        .Where(s => teacherIds.Contains(s.UserId))
                        .Select(s => s.DeviceToken)
                        .ToArrayAsync(cancellationToken);

                    await _firebaseService.Send(new FirebaseNotification(
                        tokens,
                        subject: $"You have been assigned as teacher for {course.Name}", 
                        body: "You have been assigned teacher"));
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Unable to send Firebase notification");
                }


                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return course;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(exception, "Unable to create course");
            }
        }

        return null;
    }
}

