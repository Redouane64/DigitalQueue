using DigitalQueue.Web.Areas.Sessions.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Sessions.Queries;

public partial class GetUserSessionsQuery : IRequest<IEnumerable<SessionDto>>
{
    public string UserId { get; }

    public GetUserSessionsQuery(string userId)
    {
        UserId = userId;
    }
}
