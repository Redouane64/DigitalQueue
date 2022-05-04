using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public class CreateCourseCommand : IRequest<Course?>
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
    
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Course?>
    {
        private readonly DigitalQueueContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CreateCourseCommandHandler> _logger;

        public CreateCourseCommandHandler(DigitalQueueContext context, UserManager<User> userManager, ILogger<CreateCourseCommandHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        
        public async Task<Course?> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            if (request.Teachers is null || request.Teachers.Length == 0)
            {
                return null;
            }
            
            await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var teachers = await _userManager.Users
                        .Where(u => request.Teachers.Contains(u.Id))
                        .ToArrayAsync(cancellationToken);
            
                    var course = new Course
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = request.Title, 
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
}
 
