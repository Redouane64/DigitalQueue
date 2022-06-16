using System.ComponentModel.DataAnnotations;

namespace DigitalQueue.Web.Areas.Accounts.Models;

public class VerifyCreateAuthenticationDataCode : CreateAuthenticationData
{
    [Required]
    [RegularExpression("\\d{6}")]
    [DataType(DataType.Text)]
    public string Code { get; set; }
}
