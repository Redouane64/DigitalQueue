
using System.ComponentModel.DataAnnotations;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class ChangeEmailDto
{
    [Required]
    public string Email { get; set; }
}
