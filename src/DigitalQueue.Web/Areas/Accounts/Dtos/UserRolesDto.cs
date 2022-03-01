namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UserRolesDto
{
    public IEnumerable<RoleDto> Roles { get; }
    public string UserId { get; }
    public bool Editable { get; }

    public UserRolesDto(IEnumerable<RoleDto> roles, string userId, bool editable = true)
    {
        Roles = roles;
        UserId = userId;
        Editable = editable;
    }
}
