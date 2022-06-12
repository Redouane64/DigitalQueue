using DigitalQueue.Web.Areas.Accounts.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Queries;

public partial class GetUserPermissionsQuery : IRequest<IEnumerable<UserCourseRolesDto>>
{
    public string UserId { get; }

    public GetUserPermissionsQuery(string userId)
    {
        UserId = userId;
    }
}
