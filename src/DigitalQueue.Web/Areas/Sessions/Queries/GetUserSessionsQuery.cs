using System.Security.Claims;

using DigitalQueue.Web.Areas.Sessions.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Sessions.Queries;

public class GetUserSessionsQuery : IRequest<IEnumerable<SessionDto>>
{
    public string UserId { get; }

    public GetUserSessionsQuery(string userId)
    {
        UserId = userId;
    }
    
    public class GetUserSessionQueryHandler : IRequestHandler<GetUserSessionsQuery, IEnumerable<SessionDto>>
    {
        private readonly DigitalQueueContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserSessionQueryHandler(DigitalQueueContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<IEnumerable<SessionDto>> Handle(GetUserSessionsQuery request, CancellationToken cancellationToken)
        {
            var currentSessionId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypesDefaults.Session);
            return await _context.Sessions.AsNoTracking()
                .Where(s => s.UserId == request.UserId)
                .Select(s => new SessionDto
                {
                    Id = s.Id, 
                    Current = (s.Id == currentSessionId)
                }).ToArrayAsync(cancellationToken);
        }
    }
}
