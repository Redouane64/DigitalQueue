namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UserRolesDto
{
    public IEnumerable<AccountRoleDto> Roles { get; }
    public string UserId { get; }
    public bool Editable { get; }

    public UserRolesDto(IEnumerable<AccountRoleDto> roles, string userId, bool editable = true)
    {
        Roles = roles;
        UserId = userId;
        Editable = editable;
    }
}
