using System.Security.Claims;

using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Services.MailService;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class CreatePasswordResetTokenCommand : IRequest<bool>
{
    public ClaimsPrincipal Principal { get; }

    public CreatePasswordResetTokenCommand(ClaimsPrincipal principal)
    {
        Principal = principal;
    }

    public class CreatePasswordResetTokenCommandHandler : IRequestHandler<CreatePasswordResetTokenCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly MailService _mailService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatePasswordResetTokenCommandHandler(
            UserManager<User> userManager, 
            MailService mailService, 
            LinkGenerator linkGenerator, 
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _mailService = mailService;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<bool> Handle(CreatePasswordResetTokenCommand create, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.GetUserAsync(create.Principal);
            
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var url =
                    this._linkGenerator.GetUriByPage(_httpContextAccessor.HttpContext, 
                        "/ResetPassword", 
                        null, 
                        new { token, email = user.Email, area = "Accounts" });
            
                await this._mailService.SendPasswordReset(user.Email, url);
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }
    }
}
