using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Models;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Common;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Accounts.Queries.Sessions;

public class GetUserSessionsQuery : IRequest<IEnumerable<UserSession>>
{
    public ClaimsPrincipal User { get; }

    public GetUserSessionsQuery(ClaimsPrincipal user)
    {
        User = user;
    }
}

public class GetUserSessionQueryHandler : IRequestHandler<GetUserSessionsQuery, IEnumerable<UserSession>>
{
    private readonly DigitalQueueContext _context;

    public GetUserSessionQueryHandler(DigitalQueueContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserSession>> Handle(GetUserSessionsQuery request, CancellationToken cancellationToken)
    {
        var session = request.User.FindFirstValue(ClaimTypesDefaults.Session);
        var userId = request.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        return await _context.Sessions.AsNoTracking()
            .Where(s => s.UserId == userId)
            .Select(s => new UserSession
            {
                Id = s.Id,
                Current = (s.Id == session)
            })
            .ToArrayAsync(cancellationToken);
    }
}

