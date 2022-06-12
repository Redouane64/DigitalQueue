
using DigitalQueue.Web.Areas.Courses.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public partial class GetCoursesQueuesQuery : IRequest<QueuesDto>
{
    public string UserId { get; }

    public GetCoursesQueuesQuery(string userId)
    {
        UserId = userId;
    }
}
