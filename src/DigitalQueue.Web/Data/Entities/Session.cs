namespace DigitalQueue.Web.Data.Entities;

public class Session : IBaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DeviceToken { get; set; }
    public string DeviceIP { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }
    
}
