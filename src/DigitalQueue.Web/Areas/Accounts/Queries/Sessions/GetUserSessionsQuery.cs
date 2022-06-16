using DigitalQueue.Web.Areas.Accounts.Models;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Queries.Sessions;

public partial class GetUserSessionsQuery : IRequest<IEnumerable<UserSession>>
{
    public string UserId { get; }

    public GetUserSessionsQuery(string userId)
    {
        UserId = userId;
    }
}
