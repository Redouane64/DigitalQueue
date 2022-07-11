using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Common;
using DigitalQueue.Web.Data.Users;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Accounts.Commands.Sessions;

public class DeleteSessionCommand : IRequest
{
    public string UserId { get; }
    public string SessionSecurityStamp { get; }

    public DeleteSessionCommand(string userId, string sessionSecurityStamp)
    {
        UserId = userId;
        SessionSecurityStamp = sessionSecurityStamp;
    }
}

public class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand>
{
    private readonly DigitalQueueContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<DeleteSessionCommandHandler> _logger;

    public DeleteSessionCommandHandler(DigitalQueueContext context, UserManager<ApplicationUser> userManager, ILogger<DeleteSessionCommandHandler> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var session = await _context.Sessions.Include(s => s.User).FirstOrDefaultAsync(
                s => s.UserId == request.UserId && s.SecurityStamp == request.SessionSecurityStamp, cancellationToken);

            if (session is null)
            {
                _logger.LogWarning("Session {SessionId} does not exist", request.SessionSecurityStamp);
                return Unit.Value;
            }

            _context.Remove(session);

            var deleteClaimResult =
                await _userManager.RemoveClaimAsync(session.User,
                    new Claim(ClaimTypesDefaults.Session, session.Id));

            if (!deleteClaimResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to delete session {SessionId}", request.SessionSecurityStamp);
            await transaction.RollbackAsync(cancellationToken);
        }

        return Unit.Value;
    }
}

