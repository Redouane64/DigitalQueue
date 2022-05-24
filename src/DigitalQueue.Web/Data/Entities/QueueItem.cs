namespace DigitalQueue.Web.Data.Entities;

public class QueueItem : IBaseEntity
{
    public string Id { get; set; }

    public string CourseId { get; set; }
    public Course Course { get; set; }

    public string CreatorId { get; set; }
    public User Creator { get; set; }

    public bool Completed { get; set; }

    public DateTime CreateAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
