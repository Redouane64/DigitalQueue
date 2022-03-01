namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UserPermissionsDto
{
    public IEnumerable<ClaimDto> Claims { get; }

    public UserPermissionsDto(IEnumerable<ClaimDto> claims)
    {
        Claims = claims;
    }
}
