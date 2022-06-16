namespace DigitalQueue.Web.Areas.Accounts.Services;

public interface ISessionsStore
{
    Task<string> CreateSession(IDictionary<string, string> data);
    Task<IDictionary<string, string>?> GetSession(string key);
    Task DeleteSession(string key);
}
