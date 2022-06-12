using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class UpdateNameCommand
{
    public class UpdateNameCommandHandler : IRequestHandler<UpdateNameCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly DigitalQueueContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UpdateNameCommandHandler> _logger;

        public UpdateNameCommandHandler(UserManager<User> userManager, DigitalQueueContext context,
            IHttpContextAccessor httpContextAccessor, ILogger<UpdateNameCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateNameCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                if (user is null)
                {
                    return false;
                }

                if (request.Name is not null && user.Name != request.Name)
                {
                    user.Name = request.Name;
                    _context.Entry(user).Property(u => u.Name).IsModified = true;
                    var updateResult = await _userManager.UpdateAsync(user);

                    if (!updateResult.Succeeded)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                    }

                    await transaction.CommitAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(e, "Unable to update account data");

                return false;
            }

            return true;
        }
    }
}
