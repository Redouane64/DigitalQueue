
using System.ComponentModel.DataAnnotations;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class ConfirmEmailDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Token { get; set; }
}
