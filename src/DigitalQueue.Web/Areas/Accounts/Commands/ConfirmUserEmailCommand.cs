using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class ConfirmUserEmailCommand : IRequest<bool>
{
    public string UserId { get; }
    public string Token { get; }

    public ConfirmUserEmailCommand(string userId, string token)
    {
        UserId = userId;
        Token = token;
    }
    
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmUserEmailCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ConfirmEmailCommandHandler> _logger;

        public ConfirmEmailCommandHandler(UserManager<User> userManager, ILogger<ConfirmEmailCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        
        public async Task<bool> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await this._userManager.FindByIdAsync(request.UserId);

                if (user is null)
                {
                    return false;
                }

                var result = await this._userManager.ConfirmEmailAsync(user, request.Token);

                if (!result.Succeeded)
                {
                    var error = result.Errors.Select(e => e.Description).FirstOrDefault();
                    _logger.LogError("Unable to verify e-mail address: {email}", error);
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to verify e-mail address");
                return false;
            }

            return true;
        }
    }
}
