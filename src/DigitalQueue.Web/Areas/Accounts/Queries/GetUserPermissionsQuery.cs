using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Accounts.Queries;

public class GetUserPermissionsQuery : IRequest<IEnumerable<UserCourseRolesDto>>
{
    public string UserId { get; }

    public GetUserPermissionsQuery(string userId)
    {
        UserId = userId;
    }
    
    public class GetUserPermissionQueryHandler : IRequestHandler<GetUserPermissionsQuery, IEnumerable<UserCourseRolesDto>>
    {
        private readonly DigitalQueueContext _context;

        public GetUserPermissionQueryHandler(DigitalQueueContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<UserCourseRolesDto>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            var userCoursesPermissions = await _context.Set<IdentityUserClaim<string>>()
                .Where(e => e.UserId == request.UserId && (e.ClaimType == ClaimTypesDefaults.Student ||
                                                   e.ClaimType == ClaimTypesDefaults.Teacher))
                .Join(_context.Courses.Where(c => !c.IsArchived),
                    e => e.ClaimValue,
                    c => c.Id, 
                    (e, c) => new
                    {
                        e.ClaimType,
                        c.Title,
                        c.Id
                    })
                .GroupBy(c => new {c.Id, c.Title})
                .Select(e =>
                    new UserCourseRolesDto(e.Key.Id, e.Key.Title, e.Select(c => c.ClaimType).ToArray()))
                .ToArrayAsync(cancellationToken: cancellationToken);
        
            return userCoursesPermissions;
        }
    }
}
