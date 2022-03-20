using System.Security.Claims;
using System.Text;

using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Services.MailService;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class SendEmailConfirmationCommand : IRequest<bool>
{
    public ClaimsPrincipal Principal { get; }

    public SendEmailConfirmationCommand(ClaimsPrincipal principal)
    {
        Principal = principal;
    }
    
    public class SendEmailConfirmationCommandHandler : IRequestHandler<SendEmailConfirmationCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly MailService _mailService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SendEmailConfirmationCommandHandler> _logger;

        public SendEmailConfirmationCommandHandler(
            UserManager<User> userManager,
            MailService mailService, 
            LinkGenerator linkGenerator, 
            IHttpContextAccessor httpContextAccessor,
            ILogger<SendEmailConfirmationCommandHandler> logger)
        {
            _userManager = userManager;
            _mailService = mailService;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        
        public async Task<bool> Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
        {

            try
            {
                User user = await this._userManager.GetUserAsync(request.Principal);
                
                var token = WebEncoders.Base64UrlEncode(
                    Encoding.UTF8.GetBytes(await _userManager.GenerateEmailConfirmationTokenAsync(user)));
        
                var confirmationLink =
                    this._linkGenerator.GetUriByPage(_httpContextAccessor.HttpContext, 
                        "/ConfirmEmail", 
                        null, 
                        new { token, email = user.Email, area = "Accounts" });

                await this._mailService.SendEmailConfirmation(user.Email, confirmationLink);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to send e-mail confirmation");

                return false;
            }
        }
    }
}

