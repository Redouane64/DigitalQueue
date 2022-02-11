namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public record UserDto(string email, string username, IList<string>? role);
