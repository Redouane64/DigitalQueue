namespace DigitalQueue.Web.Data.Entities;

public interface IBaseEntity
{
    public DateTime CreateAt { get; set; }

    public DateTime UpdatedAt { get; set; }

}
