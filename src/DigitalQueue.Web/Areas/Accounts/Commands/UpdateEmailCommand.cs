using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Events;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class UpdateEmailCommand : IRequest<bool>
{
    public string UserId { get; }
    public string Email { get; }

    public UpdateEmailCommand(string userId, string email)
    {
        UserId = userId;
        Email = email;
    }
    
    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly DigitalQueueContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateEmailCommandHandler> _logger;

        public UpdateEmailCommandHandler(
            UserManager<User> userManager, 
            DigitalQueueContext context,
            IMediator mediator,
            ILogger<UpdateEmailCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _mediator = mediator;
            _logger = logger;
        }
        
        public async Task<bool> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            await using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {

                try
                {

                    var user = await _userManager.FindByIdAsync(request.UserId);

                    if (user is null)
                    {
                        return false;
                    }

                    if (request.Email is not null && !user.Email.Equals(request.Email))
                    {
                        var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email);
                        var updateEmailResult = await _userManager.ChangeEmailAsync(user, request.Email, changeEmailToken);

                        if (!updateEmailResult.Succeeded)
                        {
                            await transaction.RollbackAsync(cancellationToken);
                            return false;
                        }
                        
                        var allClaims = await _userManager.GetClaimsAsync(user);
                        var emailClaim = allClaims.First(c => c.Type == ClaimTypes.Email);

                        var replaceEmailClaimResult = await _userManager.ReplaceClaimAsync(user, emailClaim,
                            new Claim(ClaimTypes.Email, request.Email));

                        if (!replaceEmailClaimResult.Succeeded)
                        {
                            await transaction.RollbackAsync(cancellationToken);
                            return false;
                        }

                        user.EmailConfirmed = false;
                        await _userManager.UpdateAsync(user);
                        
                        await _mediator.Publish(new EmailChangedEvent(user.Id, user.Email), cancellationToken);
                    }

                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogError(e, "Unable to update account data");

                    return false;
                }

            }

            return true;
        }
    }
}
