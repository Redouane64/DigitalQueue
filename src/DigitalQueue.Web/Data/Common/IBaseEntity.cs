namespace DigitalQueue.Web.Data.Common;

public interface IBaseEntity
{
    public DateTimeOffset CreateAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

}
