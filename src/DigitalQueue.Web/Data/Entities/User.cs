using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Data.Entities;

public class User : IdentityUser
{
    public ICollection<Course> Courses { get; set; }
    public ICollection<Membership> CoursesMemberships { get; set; }
    
}
