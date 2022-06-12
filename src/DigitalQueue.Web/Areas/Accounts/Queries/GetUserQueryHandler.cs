using DigitalQueue.Web.Areas.Accounts.Commands;
using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Sessions.Queries;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Accounts.Queries;

public partial class GetUserQuery
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto?>
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DigitalQueueContext _context;

        public GetUserQueryHandler(IMediator mediator, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DigitalQueueContext context)
        {
            _mediator = mediator;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var user = await this._userManager.Users
                .AsNoTracking()
                .Include(e => e.Courses)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user is null)
            {
                return null;
            }

            var roles = await _mediator.Send(
                new SearchRoleCommand(filter: await this._userManager.GetRolesAsync(user), returnIds: true));
            var permissions = await _mediator.Send(new GetUserPermissionsQuery(user.Id), cancellationToken);
            var sessions = await _mediator.Send(new GetUserSessionsQuery(user.Id), cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new UserDto(user, roles.Results, permissions) { Sessions = sessions };
        }
    }
}
