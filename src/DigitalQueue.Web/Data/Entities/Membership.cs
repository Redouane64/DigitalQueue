namespace DigitalQueue.Web.Data.Entities;

public class Membership
{
    public string CourseId { get; set; }
    public Course Course { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public bool IsTeacher { get; set; }
    
}
