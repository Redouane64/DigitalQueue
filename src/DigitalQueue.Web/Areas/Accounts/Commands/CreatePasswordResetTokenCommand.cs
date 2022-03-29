using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Services.MailService;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class CreatePasswordResetTokenCommand : IRequest<bool>
{
    public string UserId { get; }

    public CreatePasswordResetTokenCommand(string userId)
    {
        UserId = userId;
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
        
        public async Task<bool> Handle(CreatePasswordResetTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await this._mailService.SendPasswordResetCode(user.Email, token);
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }
    }
}
