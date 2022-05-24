using System.ComponentModel.DataAnnotations;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class VerifyAuthenticationCodeDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression("\\d{6}")]
    [DataType(DataType.Text)]
    public string Code { get; set; }

}
