
using System.ComponentModel.DataAnnotations;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class ConfirmEmailDto
{
    [Required]
    public string Token { get; set; }
}
