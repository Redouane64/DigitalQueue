using DigitalQueue.Web.Data.Common;
using DigitalQueue.Web.Data.Courses;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Data.Users;

public class ApplicationUser : IdentityUser, IBaseEntity
{
    public string Name { get; set; }

    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<Course> Courses { get; set; }
    public ICollection<Session> Sessions { get; set; }
}
