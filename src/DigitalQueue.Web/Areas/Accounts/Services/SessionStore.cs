using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Accounts.Services;

public class SessionStore : ISessionsStore
{
    private readonly string UserIdKeyName = "user_id";
    private readonly string AccessTokenKeyName = "access_token";
    private readonly string RefreshTokenKeyName = "refresh_token";
    private readonly string DeviceIdKeyName = "device_id";
    
    private readonly DigitalQueueContext _context;

    public SessionStore(DigitalQueueContext context)
    {
        _context = context;
    }
    
    public async Task<string> CreateSession(IDictionary<string, string> data)
    {
        var userId = data[UserIdKeyName];
        var accessToken = data[AccessTokenKeyName];
        var refreshToken = data[RefreshTokenKeyName];
        var deviceId = data[DeviceIdKeyName];

        var key = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        
        var session = new Session()
        {
            Id = key,
            UserId = userId, 
            AccessToken = accessToken, 
            RefreshToken = refreshToken, 
            DeviceToken = deviceId,
        };

        _context.Add(session);
        await _context.SaveChangesAsync();

        return key;
    }

    public async Task<IDictionary<string, string>?> GetSession(string key)
    {
        var entity = await _context.Sessions.FirstOrDefaultAsync(e => e.Id == key);

        if (entity is null)
        {
            return null;
        }

        return new Dictionary<string, string>()
        {
            [UserIdKeyName] = entity.UserId,
            [AccessTokenKeyName] = entity.AccessToken,
            [RefreshTokenKeyName] = entity.RefreshToken,
            [DeviceIdKeyName] = entity.DeviceToken,
        };
    }

    public Task DeleteSession(string key)
    {
        _context.Remove(new Session { Id = key });
        return _context.SaveChangesAsync();
    }
}
