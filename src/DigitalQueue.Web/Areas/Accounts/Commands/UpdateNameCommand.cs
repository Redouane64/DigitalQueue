using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class UpdateNameCommand : IRequest
{
    public string UserId { get; }
    public string? Name { get; }

    public UpdateNameCommand(string userId, string name)
    {
        UserId = userId;
        Name = name;
    }
    
    public class UpdateNameCommandHandler : IRequestHandler<UpdateNameCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly DigitalQueueContext _context;
        private readonly ILogger<UpdateNameCommandHandler> _logger;

        public UpdateNameCommandHandler(UserManager<User> userManager, DigitalQueueContext context, ILogger<UpdateNameCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(UpdateNameCommand request, CancellationToken cancellationToken)
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

                    if (request.Name is not null && user.Name != request.Name)
                    {
                        user.Name = request.Name;
                        _context.Entry(user).Property(u => u.Name).IsModified = true;
                        var updateResult = await _userManager.UpdateAsync(user);

                        if (!updateResult.Succeeded)
                        {
                            await transaction.RollbackAsync(cancellationToken);
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
