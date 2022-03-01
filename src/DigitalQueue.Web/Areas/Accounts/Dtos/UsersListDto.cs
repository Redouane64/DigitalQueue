namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UsersListDto
{
    public IEnumerable<UserDto> Users { get; }
    public int? Page { get; }
    public int? PageSize { get; }
    public bool FullView { get; }

    public UsersListDto(IEnumerable<UserDto> Users, int? page = null, int? pageSize = null, bool fullView = true)
    {
        this.Users = Users;
        Page = page;
        PageSize = pageSize;
        FullView = fullView;
    }
}
