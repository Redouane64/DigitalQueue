using System.ComponentModel.DataAnnotations;

namespace DigitalQueue.Web.Areas.Sessions.Dtos;

public class SessionTokenDto
{
    [Required]
    public string Token { get; set; }
}
