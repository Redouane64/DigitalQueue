using System.Text.Json.Serialization;

namespace DigitalQueue.Web.Areas.Users.Models;

public class TeacherAccount
{
    public TeacherAccount(string name, string Id)
    {
        this.Name = name;
        this.Id = Id;
    }

    [JsonPropertyName("title")]
    public string Name { get; }
    public string Id { get; }

}
