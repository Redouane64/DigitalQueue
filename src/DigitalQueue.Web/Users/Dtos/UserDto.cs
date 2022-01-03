namespace DigitalQueue.Web.Users.Dtos;

public record UserDto(string email, string username, IList<string>? role);
