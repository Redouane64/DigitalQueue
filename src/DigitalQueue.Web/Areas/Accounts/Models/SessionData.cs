using System.ComponentModel.DataAnnotations;

namespace DigitalQueue.Web.Areas.Accounts.Models;

public class SessionData
{
    [Required]
    public string Token { get; set; }
}
