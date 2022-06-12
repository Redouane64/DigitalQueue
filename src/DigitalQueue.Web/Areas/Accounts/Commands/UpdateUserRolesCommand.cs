using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class UpdateUserRolesCommand : IRequest<bool>
{
    public UpdateUserRolesCommand(string user, string[] roles, bool remove = false)
    {
        User = user;
        Roles = roles;
        Remove = remove;
    }

    public string User { get; }
    public string[] Roles { get; }
    public bool Remove { get; }
}
