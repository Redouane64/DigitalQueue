namespace DigitalQueue.Web.Data.Entities;

public class Course
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; }
    
    public ICollection<User> Teachers { get; set; }

}
