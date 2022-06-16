using System.ComponentModel.DataAnnotations;

namespace DigitalQueue.Web.Areas.Accounts.Models;

public class CreateAuthenticationData
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
