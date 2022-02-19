using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class UpdateRoleCommand : IRequest<bool>
{
    public UpdateRoleCommand(string user, string[] roles)
    {
        User = user;
        Roles = roles;
    }

    public string User { get; }

    public string[] Roles { get; }
    
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly DigitalQueueContext _context;
        private readonly ILogger<UpdateRoleCommandHandler> _logger;

        public UpdateRoleCommandHandler(UserManager<User> userManager, DigitalQueueContext context, ILogger<UpdateRoleCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        
        public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
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

                    var existingRoles = await _userManager.GetRolesAsync(user);
                    var newRoles = new List<string> { RoleDefaults.User };
                    if (request.Roles.Length < existingRoles.Count)
                    {
                        newRoles.AddRange(existingRoles.Except(request.Roles).ToArray());
                    }
                    else
                    {
                        newRoles.AddRange(request.Roles.Except(existingRoles).ToArray());
                    }

                    foreach (var role in newRoles
                                            .Where(r => !r.Equals(RoleDefaults.User, StringComparison.InvariantCultureIgnoreCase))
                                            .Distinct()
                                            .ToArray())
                    {
                        if (!await _userManager.IsInRoleAsync(user, role))
                        {
                            await _userManager.AddToRoleAsync(user, role);
                            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
                        }
                        else
                        {
                            
                            await _userManager.RemoveFromRoleAsync(user, role);
                            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, role));
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
