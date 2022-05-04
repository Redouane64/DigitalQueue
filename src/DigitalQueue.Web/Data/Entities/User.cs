using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Data.Entities;

public class User : IdentityUser, IBaseEntity
{
    public string Name { get; set; }
    
    public DateTime CreateAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public ICollection<Course> Courses { get; set; }

    public ICollection<Session> Sessions { get; set; }
    
}
