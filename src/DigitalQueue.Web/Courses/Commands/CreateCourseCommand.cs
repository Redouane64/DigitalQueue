using MediatR;

using Microsoft.Build.Framework;

namespace DigitalQueue.Web.Courses.Commands;

public class CreateCourseCommand : IRequest<object>
{
    public CreateCourseCommand(string title, string[] teachers)
    {
        Title = title;
        Teachers = teachers;
    }

    [Required]
    public string Title { get; }

    [Required]
    public string[] Teachers { get; }
    
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, object>
    {
        public Task<object> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
 
