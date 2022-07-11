using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Models;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Users;
using DigitalQueue.Web.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands.Account;

public class UpdateAccountCommand : IRequest
{
    public ClaimsPrincipal User { get; }
    public UpdateAccountData Data { get; }

    public UpdateAccountCommand(ClaimsPrincipal user, UpdateAccountData data)
    {
        User = user;
        Data = data;
    }
}

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DigitalQueueContext _context;

    public UpdateAccountCommandHandler(UserManager<ApplicationUser> userManager, DigitalQueueContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    
    public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        var user = await _userManager.GetUserAsync(request.User);
        
        if (!String.IsNullOrEmpty(request.Data.Name))
        {
            await _userManager.SetUserNameAsync(user, request.Data.Name);
        }

        if (!string.IsNullOrEmpty(request.Data.Email) && !string.IsNullOrEmpty(request.Data.Token))
        {
            var tokenIsValid = await _userManager.VerifyUserTokenAsync(
                user, 
                UserTokenProvider.ProviderName, 
                UserTokenProvider.UpdateEmailPurposeName, 
                request.Data.Token);

            if (tokenIsValid)
            {
                await _userManager.ChangeEmailAsync(user, request.Data.Email, request.Data.Token);
            }
        }

        await transaction.CommitAsync(cancellationToken);
        
        return Unit.Value;
    }
}
