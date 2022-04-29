using System.ComponentModel.DataAnnotations;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class AuthenticateUserDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}
