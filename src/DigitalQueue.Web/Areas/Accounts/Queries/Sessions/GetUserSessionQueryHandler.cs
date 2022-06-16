using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Models;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Accounts.Queries.Sessions;

public partial class GetUserSessionsQuery
{
    public class GetUserSessionQueryHandler : IRequestHandler<GetUserSessionsQuery, IEnumerable<UserSession>>
    {
        private readonly DigitalQueueContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserSessionQueryHandler(DigitalQueueContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<UserSession>> Handle(GetUserSessionsQuery request, CancellationToken cancellationToken)
        {
            var currentSessionId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypesDefaults.Session);
            return await _context.Sessions.AsNoTracking()
                .Where(s => s.UserId == request.UserId)
                .Select(s => new UserSession
                {
                    Id = s.Id,
                    Current = (s.Id == currentSessionId)
                }).ToArrayAsync(cancellationToken);
        }
    }
}
