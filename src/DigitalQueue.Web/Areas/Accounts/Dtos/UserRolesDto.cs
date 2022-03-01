namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UserRolesDto
{
    public IEnumerable<RoleDto> Roles { get; }
    public bool Editable { get; }

    public UserRolesDto(IEnumerable<RoleDto> roles, bool editable = true)
    {
        Roles = roles;
        Editable = editable;
    }
}
