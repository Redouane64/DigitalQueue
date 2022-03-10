using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class UpdateUserRolesCommand : IRequest<bool>
{
    public UpdateUserRolesCommand(string user, string[] roles, bool remove = false)
    {
        User = user;
        Roles = roles;
        Remove = remove;
    }

    public string User { get; }
    public string[] Roles { get; }
    public bool Remove { get; }

    public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly DigitalQueueContext _context;
        private readonly ILogger<UpdateUserRolesCommandHandler> _logger;

        public UpdateUserRolesCommandHandler(UserManager<User> userManager, DigitalQueueContext context, ILogger<UpdateUserRolesCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        
        public async Task<bool> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(request.User);

                    if (user is null)
                    {
                        return false;
                    }
                    
                    foreach (var role in request.Roles)
                    {
                        if (request.Remove)
                        {
                            if (await _userManager.IsInRoleAsync(user, role))
                            {
                                await _userManager.RemoveFromRoleAsync(user, role);
                                await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, role));
                            }
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, role);
                            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
                        }
                    }
                    
                    await transaction.CommitAsync(cancellationToken);

                    return true;
                }
                catch(Exception exception)
                {
                    _logger.LogError(exception, null);
                    
                    await transaction.RollbackAsync(cancellationToken);
                    
                    return false;
                }
                
            }
        }
    }
}
