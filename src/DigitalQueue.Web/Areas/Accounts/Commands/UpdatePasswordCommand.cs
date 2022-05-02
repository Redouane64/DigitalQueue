using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class UpdatePasswordCommand : IRequest<bool>
{

    public UpdatePasswordCommand(string password, string token)
    {
        Password = password;
        Token = token;
    }
    
    public string Password { get; }
    public string Token { get; }

    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdatePasswordCommandHandler(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<bool> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await this._userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            
            if (user is null)
            {
                return false;
            }
            
            var changePasswordResult = await this._userManager.ResetPasswordAsync(
                user, 
                request.Token, 
                request.Password);

            return changePasswordResult.Succeeded;
        }
    }
}
