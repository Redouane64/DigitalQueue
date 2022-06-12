using System.ComponentModel.DataAnnotations;

using DigitalQueue.Web.Data.Entities;

using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Commands;

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

