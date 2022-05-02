using System.ComponentModel.DataAnnotations;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UpdateEmailDto
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Token { get; set; }
}
