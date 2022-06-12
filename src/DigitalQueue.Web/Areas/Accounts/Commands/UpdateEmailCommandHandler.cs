using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class UpdateEmailCommand
{
    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly DigitalQueueContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UpdateEmailCommandHandler> _logger;

        public UpdateEmailCommandHandler(
            UserManager<User> userManager,
            DigitalQueueContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UpdateEmailCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            await using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);

                    if (user is null)
                    {
                        _logger.LogWarning("User does not exist");
                        return false;
                    }

                    var isValidToken = await _userManager.VerifyUserTokenAsync(user,
                        AuthenticationTokenProvider.ProviderName,
                        AuthenticationTokenProvider.AuthenticationPurposeName, request.Token);

                    if (!isValidToken)
                    {
                        return false;
                    }

                    if (!user.Email.Equals(request.Email))
                    {
                        var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email);
                        var updateEmailResult = await _userManager.ChangeEmailAsync(user, request.Email, changeEmailToken);

                        if (!updateEmailResult.Succeeded)
                        {
                            await transaction.RollbackAsync(cancellationToken);
                            return false;
                        }
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
