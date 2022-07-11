
using DigitalQueue.Web.Data.Common;
using DigitalQueue.Web.Data.Users;

namespace DigitalQueue.Web.Data.Courses;

public class QueueItem : IBaseEntity
{
    public string Id { get; set; }

    public string CourseId { get; set; }
    public Course Course { get; set; }

    public string CreatorId { get; set; }
    public ApplicationUser Creator { get; set; }

    public bool Completed { get; set; }

    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
