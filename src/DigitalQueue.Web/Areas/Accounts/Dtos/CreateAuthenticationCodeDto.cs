using System.ComponentModel.DataAnnotations;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class CreateAuthenticationCodeDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}
