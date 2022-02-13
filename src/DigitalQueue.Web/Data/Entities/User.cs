using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Data.Entities;

public class User : IdentityUser
{
    public string FullName { get; set; }
    
    public ICollection<Course> Courses { get; set; }
    public ICollection<Membership> CoursesMemberships { get; set; }
    
}
