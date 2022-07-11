using DigitalQueue.Web.Data.Common;
using DigitalQueue.Web.Data.Users;

namespace DigitalQueue.Web.Data.Courses;

public class Course : IBaseEntity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }
    public int? Year { get; set; }

    public bool Deleted { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<ApplicationUser>? Teachers { get; set; }
    public ICollection<QueueItem>? QueueItems { get; set; }
}
