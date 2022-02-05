namespace DigitalQueue.Web.Data.Entities;

public class Course
{
    public string Id { get; set; }

    public string Title { get; set; }
    
    public ICollection<User> Members { get; set; }
    public ICollection<Membership> CourseMemberships { get; set; }
    
}
