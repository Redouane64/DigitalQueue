namespace DigitalQueue.Web.Data.Entities;

public class Session : IBaseEntity
{
    public Session()
    {
        Id = Guid.NewGuid().ToString();
    }
    public string Id { get; set; }
    public string DeviceToken { get; set; }
    public string SecurityStamp { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

}
