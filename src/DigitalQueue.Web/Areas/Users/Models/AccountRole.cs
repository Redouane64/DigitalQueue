using System.Text.Json.Serialization;

namespace DigitalQueue.Web.Areas.Users.Models;

public class AccountRole
{
    public AccountRole(string name, string id)
    {
        this.Name = name;
        this.Id = id;
    }

    [JsonPropertyName("text")]
    public string Name { get; }
    public string Id { get; }
}
