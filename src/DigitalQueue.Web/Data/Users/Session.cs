using DigitalQueue.Web.Data.Common;

namespace DigitalQueue.Web.Data.Users;

public class Session : IBaseEntity
{
    public string Id { get; set; }
    public string DeviceToken { get; set; }
    public string SecurityStamp { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

}
