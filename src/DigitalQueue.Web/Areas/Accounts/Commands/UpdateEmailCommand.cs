using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Events;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class UpdateEmailCommand : IRequest
{
    public string UserId { get; }
    public string Email { get; }

    public UpdateEmailCommand(string userId, string email)
    {
        UserId = userId;
        Email = email;
    }
    
    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand>
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
        
        public async Task<Unit> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            await using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {

                try
                {

                    var user = await _userManager.FindByIdAsync(request.UserId);

                    if (user is null)
                    {
                        return await Unit.Task;
                    }

                    if (request.Email is not null && !user.Email.Equals(request.Email))
                    {
                        var updateResult = await _userManager.SetEmailAsync(user, request.Email);
                        if (updateResult.Succeeded)
                        {
                            var allClaims = await _userManager.GetClaimsAsync(user);
                            var emailClaim = allClaims.First(c => c.Type == ClaimTypes.Email);

                            var result = await _userManager.ReplaceClaimAsync(user, emailClaim,
                                new Claim(ClaimTypes.Email, request.Email));

                            if (!result.Succeeded)
                            {
                                await transaction.RollbackAsync(cancellationToken);
                            }
                            else
                            {
                                await _mediator.Publish(new EmailChangedEvent(user.Id, user.Email));
                            }
                        }

                    }

                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogError(e, "Unable to update account data");
                }

            }

            return await Unit.Task;
        }
    }
}
