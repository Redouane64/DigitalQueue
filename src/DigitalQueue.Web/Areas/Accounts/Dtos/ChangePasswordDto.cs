
using System.ComponentModel.DataAnnotations;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class ChangePasswordDto
{
    [Required]
    public string Password { get; set; }
    [Required]
    public string Token { get; set; }
}
