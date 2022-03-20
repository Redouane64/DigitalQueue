using System.Text;
using System.Text.Encodings.Web;

using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class ConfirmEmailCommand : IRequest<bool>
{
    public string Email { get; }
    public string Token { get; }

    public ConfirmEmailCommand(string email, string token)
    {
        Email = email;
        Token = token;
    }
    
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ConfirmEmailCommandHandler> _logger;

        public ConfirmEmailCommandHandler(UserManager<User> userManager, ILogger<ConfirmEmailCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        
        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await this._userManager.FindByEmailAsync(request.Email);

                if (user is null)
                {
                    return false;
                }
                
                
                var result = await this._userManager.ConfirmEmailAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token)));

                if (!result.Succeeded)
                {
                    _logger.LogError("Unable to verify e-mail address: {0}", result.Errors.Select(e => e.Description).FirstOrDefault());
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
