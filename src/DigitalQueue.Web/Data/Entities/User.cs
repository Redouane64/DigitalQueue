using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Data.Entities;

public class User : IdentityUser, IBaseEntity
{
    public string? FullName { get; set; }
    
    public DateTime CreateAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public ICollection<Course>? TeacherOf { get; set; }
    
}
