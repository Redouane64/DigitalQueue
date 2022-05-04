namespace DigitalQueue.Web.Data.Entities;

public class Course : IBaseEntity
{
    public string Id { get; set; }

    public string Title { get; set; }

    public int Year { get; set; }
    
    public bool IsArchived { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public ICollection<User>? Teachers { get; set; }

    public ICollection<QueueItem>? QueueItems { get; set; }
}
